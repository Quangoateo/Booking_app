using Microsoft.EntityFrameworkCore;
using BookingApp.Data;
using BookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApp.Helpers;
using BookingApp.DTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using NetUtility;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BookingApp.DTO.auth;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using BookingApp.Constants;
using Microsoft.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;
using Novell.Directory.Ldap;
using System.DirectoryServices;
using Novell.Directory.Ldap.Utilclass;
using System.Text.RegularExpressions;
using Syncfusion.JavaScript.Models;

namespace BookingApp.Services
{
    public interface ILdapService
    {
        Task<OperationResult> LoginAsync(UserForLoginDto loginDto);
    }
    public class LdapService : ILdapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        static HttpClient client = new HttpClient();
        private readonly string _ldapServer;
        private readonly int _ldapPort;
        private readonly string _ldapBindDn;
        private readonly string _ldapBindCredentials;
        private readonly string _ldapSearchBase;
        private readonly string _ldapSearchFilter;
        private readonly IRepositoryBase<User> _repoUser;
        private readonly IRepositoryBase<Role> _repoRole;
        private readonly IMapper _mapper;

        public LdapService(
            IUnitOfWork unitOfWork,
            IUserService userService,
            IAuthService authService,
            IRepositoryBase<User> repoUser,
            IRepositoryBase<Role> repoRole,
            JwtSettings jwtSettings,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            TokenValidationParameters tokenValidationParameters,
            IWebHostEnvironment currentEnvironment,
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            var ldapConfig = _configuration.GetSection("LdapSettings");
            _ldapServer = ldapConfig.GetValue<string>("Server");
            _ldapPort = ldapConfig.GetValue<int>("Port");
            _ldapBindDn = ldapConfig.GetValue<string>("BindDn");
            _ldapBindCredentials = ldapConfig.GetValue<string>("BindCredentials");
            _ldapSearchBase = ldapConfig.GetValue<string>("SearchBase");
            _ldapSearchFilter = ldapConfig.GetValue<string>("SearchFilter");
            _repoUser = repoUser;
            _repoRole = repoRole;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _mapper = mapper;
            _userService = userService;
            _authService = authService;
            _currentEnvironment = currentEnvironment;
           
        }

        public bool ValidateCredentials(string username, string password)
        {
            using (var ldapConnection = new LdapConnection())
            {
                try
                {
                    ldapConnection.Connect(_ldapServer, _ldapPort);
                    ldapConnection.Bind(_ldapBindDn, _ldapBindCredentials);
                    var searchFilter = string.Format(_ldapSearchFilter, username);
                    var searchResult = ldapConnection.Search(
                        _ldapSearchBase,
                        LdapConnection.ScopeSub,
                        searchFilter,
                        null,
                        false
                    );
                    if (searchResult.HasMore())
                    {
                        // Lấy thông tin người dùng
                        var nextEntry = searchResult.Next();
                        var userDn = nextEntry.Dn;

                        // Thực hiện xác thực mật khẩu của người dùng
                        ldapConnection.Bind(userDn, password);

                        // Nếu không có ngoại lệ ném ra, tức là xác thực thành công
                        return true;
                    }
                    else
                    {
                        // Không tìm thấy người dùng
                        return false;
                    }
                }
                catch (LdapException ex)
                {
                    return false;
                }
                finally
                {
                    ldapConnection.Disconnect();
                }
            }
        }

        public async Task<OperationResult> LoginAsync(UserForLoginDto model)
        {
            if (ValidateCredentials(model.Username, model.Password))
            {
                using (var ldapConnection = new LdapConnection())
                {
                    string[] memberOf = new string[] { "MemberOf", "Name", "sAMAccountName", "mail" };
                    try
                    {
                        ldapConnection.Connect(_ldapServer, _ldapPort);
                        ldapConnection.Bind(_ldapBindDn, _ldapBindCredentials);
                        var searchFilter = string.Format(_ldapSearchFilter, model.Username);
                        var searchResult = ldapConnection.Search(
                            _ldapSearchBase,
                            LdapConnection.ScopeSub,
                            searchFilter,
                            null,
                            false
                        );
                        var userInfo = new User();

                        if (searchResult.HasMore())
                        {

                            var nextEntry = searchResult.Next();
                            userInfo.Name = nextEntry.GetAttribute("Name").StringValue;
                            //userInfo.RoleGuid = _repoRole.FindSingle(x => x.RoleName == "User").ToSafetyString();
                            userInfo.RoleId = _repoRole.FindAll(x => x.RoleName == "User" ).FirstOrDefault().ID;
                            userInfo.LdapLogin = true;
                            userInfo.Email = nextEntry.GetAttribute("mail").StringValue;
                            userInfo.LdapName = nextEntry.GetAttribute("sAMAccountName").StringValue;
                            //userInfo.StaffCode = nextEntry.GetAttribute("objectSid")
                            var depart = nextEntry.GetAttributeSet("MemberOf").Count > 0 ? nextEntry.GetAttribute("MemberOf").StringValueArray[1] : string.Empty;
                            var match = Regex.Match(depart, @"CN=(?<department>[^,]+)");
                            if (match.Success) { userInfo.Department = match.Groups["department"].Value; }
                            //var depart = nextEntry.GetAttribute("MemberOf").StringValueArray[1].Split(",")[0].Split("=")[1];

                            var item = _mapper.Map<UserDto>(userInfo);
                            // check user exist
                            var existAccount =  _repoUser.FindSingle(x => x.LdapName == userInfo.LdapName);

                            if (existAccount == null) // if user not exist -> add new user
                            {
                                item.Status = true;
                                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                                await _userService.AddAsync(item);
                            }
                            else // if user exist -> update user
                            {
                                item.ID = existAccount.ID;
                                item.RoleId = existAccount.RoleId;
                                await _userService.UpdateAsync(item);
                            }
                            var result = await _authService.LoginWithLdapAsync(userInfo.LdapName);
                            return result;
                        }

                    }
                    catch (LdapException ex)
                    {
                        return null;
                    }
                    finally
                    {
                        ldapConnection.Disconnect();
                    }
                }
            }else
            {
                return new OperationResult
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = "The Username or password is incorrect!",
                    Success = false
                };
            }

            return new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true
            };

        }

        //public User GetUserInfo(string username)
        //{
        //    //using (var ldapConnection = new LdapConnection())
        //    //{
        //    //    string[] memberOf = new string[] { "MemberOf","Name", "sAMAccountName","mail" };
        //    //    try
        //    //    {
        //    //        ldapConnection.Connect(_ldapServer, _ldapPort);
        //    //        ldapConnection.Bind(_ldapBindDn, _ldapBindCredentials);
        //    //        var searchFilter = string.Format(_ldapSearchFilter, username);
        //    //        var searchResult = ldapConnection.Search(
        //    //            _ldapSearchBase,
        //    //            LdapConnection.ScopeSub,
        //    //            searchFilter,
        //    //            memberOf,
        //    //            false
        //    //        );
        //    //        var userInfo = new User();

        //    //        if (searchResult.HasMore())
        //    //        {
                        
        //    //            var nextEntry = searchResult.Next();
        //    //            userInfo.Name = nextEntry.GetAttribute("Name").StringValue;
        //    //            userInfo.Role = SystemRole.USER;
        //    //            userInfo.LdapLogin = true;
        //    //            userInfo.Email = nextEntry.GetAttribute("mail").StringValue;
        //    //            userInfo.Ldap_Name = nextEntry.GetAttribute("sAMAccountName").StringValue;
        //    //            var depart = nextEntry.GetAttribute("MemberOf").StringValueArray[1];
        //    //            var match = Regex.Match(depart, @"CN=(?<department>[^,]+)");
        //    //            if (match.Success){ userInfo.Department = match.Groups["department"].Value; }
        //    //            //var depart = nextEntry.GetAttribute("MemberOf").StringValueArray[1].Split(",")[0].Split("=")[1];
        //    //            // check user exist
        //    //            var existAccount = _userService.CheckExistLdap(userInfo.Ldap_Name);

        //    //            var item = _mapper.Map<User>(userInfo);
        //    //            item.Status = true;
        //    //            item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();

        //    //            if (!existAccount.Success)
        //    //            {
        //    //                await _accountService.AddFormAsync(model);
        //    //            }
        //    //            else
        //    //            {
        //    //                await _accountService.UpdateFormAsync(model);
        //    //            }
        //    //            _repoUser.Add(item);
        //    //            _unitOfWork.SaveChangeAsync();
        //    //            return item;
        //    //        }

        //    //        return userInfo;
        //    //    }
        //    //    catch (LdapException ex)
        //    //    {
        //    //        return null;
        //    //    }
        //    //    finally
        //    //    {
        //    //        ldapConnection.Disconnect();
        //    //    }
        //    //}
        //}

    }
}

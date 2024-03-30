import React, {useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import {toast, ToastContainer} from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import loginBgImage from "../../assets/img/login-bg.jpg";
import loginLogo from "../../assets/img/VGU-logo.png";
import "./login.css";
import {POST_LDAP_AUTH} from "../../api/apiService";
import {ILogin} from "../../api/Models";
import {ILoginResponseDTO} from "../../api/Dto.ts";
import {Circles } from 'react-loader-spinner'
const Login: React.FC = () => {
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [rememberMe, setRememberMe] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false); 
    const navigate = useNavigate();

    const handleLogin = async () => {
        setLoading(true);
        try {
            const object: ILogin = {
                username: username,
                password: password
            };
            const response = await POST_LDAP_AUTH(object);
            console.log(response);
            if (response.data.statusCode === 200) {// Logic for successful login, e.g., redirect to dashboard
                console.log("Successfully logged in");
                const userInfo: ILoginResponseDTO = response.data.data;
                // -> set the token and User info to local storage
                localStorage.setItem("token", userInfo.token);
                localStorage.setItem("userInfo", JSON.stringify(userInfo.user));

                navigate("/room-booking");
                toast.info("Login successful!")
                console.log(userInfo); //test done

            } else if (response.data.statusCode === 401) {
                // Unauthorized: Invalid credentials
                toast.error("Invalid username or password. Please try again.");
            } else {
                // Handle other error cases
                toast.error("An error occurred. Please try again later.");
            }
        } catch (error) {
            console.error("Error:", error);
            toast.error("An error occurred. Please try again later.");
        }
        setLoading(false);
    };


    return (
        <div className="grid wide login-container">
            <div className="row login-container-wrapper">
                <div className="col l-4 m-0 c-0">
                    <div className="login-form-container">
                        <img src={loginLogo} alt="VGU Logo" className="login-form-logo"/>
                        <div>
                            <h1 className="login-form-title">VGU Booking App</h1>
                            <p className="login-form-subtitle">Please Sign in to continue</p>
                        </div>

                        <form
                            className="login-form-content"
                            onKeyUp={(e) => {
                              if (e.key === "Enter") {
                                handleLogin();
                              }
                            }}
                        >
                            <label className="login-form-label" htmlFor="username">
                                Username:
                            </label>
                            <input
                                type="string"
                                id="username"
                                value={username}
                                className="login-form-input"
                                onChange={(e) => setUsername(e.target.value)}
                                placeholder="Enter your id"
                            />
                            <label className="login-form-label" htmlFor="password">
                                Password:
                            </label>
                            <input
                                type="password"
                                id="password"
                                value={password}
                                className="login-form-input"
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Enter your password"
                            />
                            <div className="login-remember-forgot-container">
                                <div className="login-remember-me">
                                    <input
                                        type="checkbox"
                                        id="rememberMe"
                                        checked={rememberMe}
                                        className="login-remember-checkbox"
                                        onChange={() => setRememberMe(!rememberMe)}
                                    />
                                    <label className="login-remember-label" htmlFor="rememberMe">
                                        Remember me
                                    </label>
                                </div>
                                <div className="login-forgot-password">
                                    <Link to="/forgot-password">Forgot Password?</Link>
                                </div>
                            </div>
                            <button
                                className="button login-form-button"
                                type="button"
                                onClick={handleLogin}
                            >
                                {
                                loading ? 
                                <div className="circle_center">
                                <Circles
                                height="26"
                                width="80"
                                color="#ffffff"
                                ariaLabel="circles-loading"
                                visible={true} /></div> : "Login"
                                }
                            </button>

                        </form>
                    </div>
                </div>
                <div className="col l-8 m-12 c-12">
                    <div className="login-image-container">
                        <img
                            src={loginBgImage}
                            alt="Login Background"
                            className="login-img"
                        />
                    </div>
                </div>
            </div>
            <ToastContainer/>
        </div>
    );
};

export default Login;

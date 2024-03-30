import React, {useState} from "react";
import {Layout, Menu, Button, Breadcrumb} from "antd";
import {
    ToolOutlined,
    BookOutlined,
    UnorderedListOutlined,
    FileProtectOutlined,
    QuestionCircleOutlined,
    BellOutlined,

} from "@ant-design/icons";
import {Link, useNavigate} from "react-router-dom";
import VGULogo from "../../assets/img/VGU-logo2.png";
import "./sidenav.css";
import {Content, Footer, Header} from "antd/es/layout/layout";
import {Outlet} from "react-router";

const {Sider} = Layout; // Destructuring Ant Design Layout component

const Sidenav: React.FC = () => {
    const [collapsed, setCollapsed] = useState(false);

    // const toggleCollapsed = () => {
    //     setCollapsed(!collapsed);
    // };

    const navigate = useNavigate();
    const LogOut = () => {
        localStorage.clear();
        navigate("/login");
    };
    const userName = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")!).name : "";
    const userRole = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")!).roleName : "";
    const userMail = localStorage.getItem("userInfo") ? JSON.parse(localStorage.getItem("userInfo")!).email : "";
    return (
        <Sider
            // className="sidenav-container"
            collapsible
            collapsed={collapsed}
            onCollapse={(value) => setCollapsed(value)}
            // width={300}
            theme={"light"}
        >
            <div className="sidenav-logo-name-container">
                <Link to="/">
                    <img src={VGULogo} alt="Logo" className="sidenav-logo"/>
                </Link>
                <div className="sidenav-app-name">VGU Booking App</div>
            </div>

            <Menu
                mode="vertical"
                theme="light"
                defaultSelectedKeys={["1"]}
                // className="sidenav-menu-bar"
            >
                <Menu.SubMenu key="booking" icon={<BookOutlined/>} title="Booking">
                    <Menu.Item key="roomBooking">
                        <Link to="/room-booking">Room Booking</Link>
                    </Menu.Item>
                    <Menu.Item key="vehicleBooking">
                        <Link to="/vehicle-booking">Vehicle Booking</Link>
                    </Menu.Item>
                </Menu.SubMenu>

                {/* My Booking Section */}
                <Menu.SubMenu
                    key="myBooking"
                    icon={<UnorderedListOutlined/>}
                    title="My Booking List"    className="sider-menu-submenu"
                >

                    <Menu.Item key="roomBookingList">
                        <Link to="/room-booking-list">Room Booking List</Link>
                    </Menu.Item>
                    <Menu.Item key="carBookingList">
                        <Link to="/car-booking-list">Car Booking List</Link>
                    </Menu.Item>
                </Menu.SubMenu>

                {/* Management Section */}
                <Menu.SubMenu
                    key="management"
                    icon={<ToolOutlined/>}
                    title="Management"
                >
                    <Menu.Item key="userManagement">
                        <Link to="/user-management">User Management</Link>
                    </Menu.Item>
                    <Menu.Item key="bookingRequests">
                        <Link to="/booking-requests">Booking Requests</Link>
                    </Menu.Item>
                    <Menu.Item key="appConfigManagement">
                        <Link to="/app-config-management">App Configuration</Link>
                    </Menu.Item>
                </Menu.SubMenu>

                {/* Regulation Section */}
                <Menu.Item key="regulation" icon={<FileProtectOutlined/>}>
                    <Link to="/regulation">Regulation</Link>
                </Menu.Item>

                {/* Help Section */}
                <Menu.Item key="help" icon={<QuestionCircleOutlined/>}>
                    <Link to="/help">Help</Link>
                </Menu.Item>

                {/* Notification Section */}
                <Menu.Item key="notification" icon={<BellOutlined/>}>
                    <Link to="/notification">Notification</Link>
                </Menu.Item>

                {/* Profile Section */}
                <div className="sidenav-profile-section">
                    <div className="sidenav-profile-section-wrapper">
                        <i className="bx bxs-user"></i>
                        <div className="sidenav-profile-user-info">
                            <div className="sidenav-user-details">
                                <div className="sidenav-user-name">{userName}</div>
                                <div className="sidenav-user-role">{userRole}</div>
                            </div>
                            <div className="sidenav-user-email">{userMail}</div>
                        </div>
                        <div className="sidenav-signout-btn">
                            <i onClick={LogOut} className="bx bx-log-out"></i>
                        </div>
                    </div>
                </div>
            </Menu>
        </Sider>


    );
};

export default Sidenav;

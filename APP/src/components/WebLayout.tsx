import React from "react";
import {Layout} from "antd";
import {Content, Footer} from "antd/es/layout/layout";
import {Outlet, useLocation} from "react-router";
import Sidenav from "./sidenav/Sidenav.tsx";

const WebLayout: React.FC = () => {
    const location = useLocation();

    // Define the routes where the layout should not be displayed
    const excludedRoutes = ["/login"]

    // Check if the current route is excluded
    const isExcludedRoute = excludedRoutes.includes(location.pathname);

    return (
        <Layout>
            {!isExcludedRoute && <Sidenav />}
            <Layout>
                <Content style={{ margin: '0', backgroundColor: 'var(--container-color)', overflowY:'auto' }}>
                    <Outlet />
                </Content>
                {!isExcludedRoute && <Footer style={{ textAlign: 'center' }}>
                    add this later
                </Footer>}
            </Layout>
        </Layout>
    );
};

export default WebLayout;
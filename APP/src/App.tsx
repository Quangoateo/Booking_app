// src/App.tsx
import React from "react";
import "./App.css";
import Sidenav from "./components/sidenav/Sidenav";
import { Outlet, useLocation } from "react-router";
import WebLayout from "./components/WebLayout.tsx";

const App: React.FC = () => {
  const location = useLocation();

  // Define the routes where the side navigation should not be displayed
  const excludedRoutes = ["/login"];

  // Check if the current route is excluded
  const isExcludedRoute = excludedRoutes.includes(location.pathname);

  return (
    <div className="app-container">
      {/*{!isExcludedRoute && <WebLayout />}*/}
        <WebLayout />
      {/*<div className="main-content">*/}
      {/*  <Outlet />*/}
      {/*</div>*/}
    </div>
  );
};
export default App;

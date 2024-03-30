import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import HomePage from "../Pages/HomePage/HomePage";
import VehicleBooking from "../components/vehiclebooking/VehicleBooking";
import Sidenav from "../components/sidenav/Sidenav";
import RoomBookingList from "../Pages/roombookinglist/RoomBookingList";
import UserManagement from "../components/usermanagement/UserManagement";
import ForgotPass from "../components/forgotpass/Forgotpass";
import Login from "../Pages/LoginPage/Login";
// import helpPage from "../Pages/HelpPage/helpPage";
import Regulation from "../Pages/RegulationPage/Regulation";
import RoomBooking from "../Pages/roombooking/RoomBooking";
import RoomInfo from "../components/roominfo/RoomInfo";
import CreateBooking from "../Pages/createbooking/createBooking";
import BookingRequests from "../Pages/bookingrequests/BookingRequests";
// import NotiPage from "../Pages/NotiPage/NotiPage";


export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      // add more routes when needed
      { path: "/", element: <HomePage /> },
      { path: "/login", element: <Login /> },
      { path: "/forgot-password", element: <ForgotPass /> },
      { path: "/sidenav", element: <Sidenav /> },
      { path: "/vehicle-booking", element: <VehicleBooking /> },
      { path: "/room-booking/", element:<RoomBooking /> },
      { path: "/room-booking-list", element: <RoomBookingList /> },
      { path: "/user-management", element: <UserManagement /> },
      { path: "/booking-requests", element: <BookingRequests />},
      // { path: "/help", element: <helpPage /> },
      { path: "/regulation", element: <Regulation /> },
      { path: "/rooms/:id", element: <RoomInfo /> },
      { path: "/rooms/:roomId/create-booking", element: <CreateBooking/>}
      // { path: "/notification", element: <NotiPage /> },
    ],
  },
]);

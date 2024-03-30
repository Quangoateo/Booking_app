// src/auth/ForgotPassword.tsx
import React, { useState } from "react";
import { Link } from "react-router-dom";
import "./forgotpass.css";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import loginBgImage from "../../assets/img/login-bg.jpg";
import loginLogo from "../../assets/img/VGU-logo.png";

const ForgotPass: React.FC = () => {
  const [email, setEmail] = useState<string>("");

  // Function to validate email format
  const isValidEmail = (value: string) => {
    // A simple regex pattern for basic email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(value);
  };
  const handleSendRecoveryRequest = () => {
    // Check if the entered email has a valid format
    if (!isValidEmail(email)) {
      // Show a notification for an invalid email format
      toast.error("Invalid email format. Please enter a valid email address.");
    } else {
      // Logic for sending the recovery request
      // This could involve an API call or other logic
      toast.success("Recovery request sent successfully!");
    }
  };


  return (
    <div className="grid wide forgot-password-container">
      <div className="row forgot-password-wrapper">
        <div className="col l-4 m-0 c-0">
          <div className="forgot-password-form-container">
            <img
              src={loginLogo}
              alt="VGU Logo"
              className="forgot-password-logo"
            />
            <div>
              <h1 className="forgot-password-form-title">VGU Booking App</h1>
              <p className="forgot-password-form-subtitle">
                Password Recovery Request
              </p>
            </div>

            <form className="forgot-password-form-content">
              <label className="forgot-password-label" htmlFor="email">
                Email address:
              </label>
              <input
                type="email"
                id="email"
                value={email}
                className="forgot-password-input"
                onChange={(e) => setEmail(e.target.value)}
                placeholder="Enter your email"
              />
              <p className="forgot-password-login-link">
                Remember your password? <Link to="/login">Login</Link>
              </p>
              <button
                className="button forgot-password-button"
                type="button"
                onClick={handleSendRecoveryRequest}
              >
                Send recovery request
              </button>
            </form>
          </div>
        </div>
        <div className="col l-8 m-12 c-12">
          <div className="forgot-password-image-container">
            <img
              src={loginBgImage}
              alt="Login Background"
              className="login-img"
            />
          </div>
        </div>
      </div>
      <ToastContainer />
    </div>
  );
};

export default ForgotPass;

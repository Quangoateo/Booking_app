import React from "react";
import "./regulation.css";

const Regulation: React.FC = () => {
  return (
    <div className="regulation-page">
      <div className="content">
        <h1>Regulations for Using Booking App Website</h1>
        <p>
          Welcome to our booking app website. Before you start using our
          services, please read and understand the following regulations
          carefully:
        </p>
        <h2>1. Account Registration</h2>
        <p>
          In order to use our booking services, you may be required to have an
          account from LDAP.
        </p>
        <h2>2. Booking Process</h2>
        <p>
          When booking through our website, you agree to provide accurate
          information regarding your booking requirements. Any false information
          provided may result in cancellation of your booking without refund.
        </p>
        <h2>3. Payment</h2>
        <p>
          In case of car booking, payment for bookings must be made through the
          provided payment methods. Failure to provide payment may result in
          cancellation of your booking.
        </p>
        <h2>4. Cancellation and Refunds</h2>
        <p>
          Cancellation policies vary depending on the booking. Please refer to
          the specific booking details for information regarding cancellation
          and refunds.
        </p>
        <h2>5. Prohibited Activities</h2>
        <p>
          The following activities are strictly prohibited on our website:
          <ul>
            <li>
              Use of automated systems or software to extract data from our
              website
            </li>
            <li>
              Posting or transmitting any unlawful, threatening, abusive,
              defamatory, obscene, or indecent information
            </li>
            <li>
              Unauthorized access or use of our website's systems or networks
            </li>
          </ul>
        </p>
        <h2>6. Contact Us</h2>
        <p>
          If you have any questions or concerns regarding these regulations or
          the use of our website, please contact us at aibiet@dungtim.com.
        </p>
      </div>
      <div className="footer">
        <p>© 2024 VGU. All rights reserved.</p>
      </div>
    </div>
  );
};

export default Regulation;

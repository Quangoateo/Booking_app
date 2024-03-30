import React from "react";
import "./helppage.css"; // Import your CSS file
import { Link } from "react-router-dom";
import { Button } from "antd";

const helpPage: React.FC = () => {
  return (
    <div className="help-page">
      <h1>Help</h1>
      <div className="section">
        <h2>Authentication</h2>
        <p>
          <strong>How do I login?</strong>
        </p>
        <p>
          The VGU Library room booking system uses LDAP to login, which means
          you login with the same username and password as you use for your
          email, e.g., studentID and MyPassword. The username does not include
          the suffix of your email.
        </p>
        <p>
          <strong>Why can't I delete/alter a meeting?</strong>
        </p>
        <p>
          In order to delete or alter a meeting, you must be logged in as the
          same person that made the meeting. Contact the VGU Library to have it
          deleted or changed.
        </p>
      </div>
      <div className="section">
        <h2>Making/Altering Meetings</h2>
        <p>
          <strong>How do I create a meeting?</strong>
        </p>
        <p>
          Clicking on the desired time brings you to the booking screen. Fill in
          the details and click "Save". Instead of clicking on the desired time,
          you can select a time range and also a room or day range by dragging
          the cursor over a group of cells in the day or week views. When the
          mouse is released, you will be taken to the booking screen, and the
          time slots and the room or day range will have been filled in for you.
        </p>
        <p>
          <strong>How do I delete one instance of a recurring meeting?</strong>
        </p>
        <p>My meeting failed to be created because of too many entries!</p>
        <p>
          <strong>
            What happens if multiple people schedule the same meeting?
          </strong>
        </p>
        <p>
          The short answer is: The first person to click on the Submit button
          wins.
        </p>
      </div>
      <div className="section">
        <h2>Direct Contact</h2>
        <p>
          <strong>
            Incase you need to communicate with us {">"}.{"<"}
          </strong>
        </p>
        <Button>
          <Link className="link-to-click" to="https:/vgu.edu.vn">
            Click here :P
          </Link>
        </Button>
      </div>
    </div>
  );
};

export default helpPage;

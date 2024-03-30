import React, { useState } from "react";
import { Layout, Table, Button, Modal, Form, Input, Select } from "antd";
import "./bookingrequests.css"; // Import CSS for styling
import requestsData from "./requestsData"; // Import CSS for styling

const { Content } = Layout;
const { Option } = Select;

const BookingRequests: React.FC = () => {
  const [bookings, setBookings] = useState<any[]>(requestsData);

  const columns = [
    {
      title: "No",
      dataIndex: "number",
      key: "number",
    },
    {
        title: "From",
        dataIndex: "from",
        key: "from",
      },
    {
      title: "Reason",
      dataIndex: "reason",
      key: "reason",
    },
    {
      title: "Location",
      dataIndex: "location",
      key: "location",
    },
    {
      title: "Date",
      dataIndex: "date",
      key: "date",
    },
    {
      title: "Time",
      dataIndex: "time",
      key: "time",
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      render: (status: string) => (
        <span className={`status-${status.toLowerCase()}`}>{status}</span>
      ),
    },
    {
      title: "Action",
      key: "action",
      render: (text: any, record: any) => (
        <span>
          {(record.status === "Pending" || record.status === "Open") && (
            <>
              <Button
                type="link"
                className="accept-button"
                onClick={() => handleAccept(record)}
              >
                Accept
              </Button>
              <Button
                type="link"
                className="reject-button"
                onClick={() => handleDecline(record)}
              >
                Reject
              </Button>
            </>
          )}
        </span>
      ),
    },
  ];

  const handleAccept = (record: any) => {
    // Update the status of the selected booking to "accepted" only if the current status is "Pending"
    if (record.status === "Pending") {
      const updatedBookings = bookings.map((booking) =>
        booking.number === record.number ? { ...booking, status: "Accepted" } : booking
      );
      setBookings(updatedBookings);
    }
  };

  const handleDecline = (record: any) => {
    // Update the status of the selected booking to "declined" only if the current status is "Pending"
    if (record.status === "Pending") {
      const updatedBookings = bookings.map((booking) =>
        booking.number === record.number ? { ...booking, status: "Rejected" } : booking
      );
      setBookings(updatedBookings);
    }
  };

  
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Layout className="site-layout">
        <Content style={{ margin: "0px" }}>
          <Table columns={columns} dataSource={bookings} rowKey="number" />
        </Content>
      </Layout>
    </Layout>
  );
};

export default BookingRequests;

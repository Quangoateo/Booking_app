import React, { useState } from "react";
import { Layout, Table, Button, Modal, Form, Input, Select } from "antd";
import "./roombookinglist.css"; // Import CSS for styling
import roomBookingData from "./roomBookingData";

const { Content } = Layout;
const { Option } = Select;

const RoomBookingList: React.FC = () => {
  const [isEditModalVisible, setIsEditModalVisible] = useState<boolean>(false);
  const [isCancelModalVisible, setIsCancelModalVisible] = useState<boolean>(false);
  const [selectedBooking, setSelectedBooking] = useState<any>(roomBookingData);

  // Edit logics
  const handleEdit = (record: any) => {
    setSelectedBooking(record);
    setIsEditModalVisible(true);
  };

  const handleSaveEdit = (record: any) => {
    if (record.status === "Accepted" || record.status === "Open") {
      const updatedBookings = selectedBooking.map((booking) =>
        booking.number === record.number ? { ...booking, status: "Pending" } : booking
      );
      setSelectedBooking(updatedBookings);
      setIsEditModalVisible(false);
      console.log("Updated", selectedBooking);
    }
  };

  // Cancel Logics
  const handleCancel = (record: any) => {
    setSelectedBooking(record);
    setIsCancelModalVisible(true);
  };

  const handleConfirmCancel = () => {
    console.log("Cancel", selectedBooking);
    setIsCancelModalVisible(true);
  };


  const columns = [
    {
      title: "No",
      dataIndex: "number",
      key: "number",
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
          {["Rejected", "Canceled"].includes(record.status) ? null : (
            <>
              <Button
                type="link"
                className="edit-button"
                onClick={() => handleEdit(record)}
              >
                Edit
              </Button>
              <Button
                type="link"
                className="cancel-button"
                onClick={() => handleCancel(record)}
              >
                Cancel
              </Button>
            </>
          )}
        </span>
      ),
    },
  ];

  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Layout className="site-layout">
        <Content style={{ margin: "0px" }}>
          <Table
            columns={columns}
            dataSource={roomBookingData}
            rowKey="number"
          />

          {/* Edit Booking Modal */}
          <Modal
            title="Edit Booking"
            open={isEditModalVisible}
            onCancel={() => setIsEditModalVisible(false)}
            onOk={handleSaveEdit}
          >
            {/* Form for editing booking */}
            <Form>
              <Form.Item label="Number">
                <Input value={selectedBooking?.number} disabled />
              </Form.Item>
              <Form.Item label="Reason of Booking">
                <Input value={selectedBooking?.reason} />
              </Form.Item>
              <Form.Item label="Location">
                <Input value={selectedBooking?.location} disabled />
              </Form.Item>
              <Form.Item label="Date">
                <Input value={selectedBooking?.date} />
              </Form.Item>
              <Form.Item label="Time">
                <Input value={selectedBooking?.time} />
              </Form.Item>
              <Form.Item label="Status">
                <Select value={selectedBooking?.status} disabled>
                  {/* Include options for different status values */}
                  <Option value="accepted">Accepted</Option>
                  <Option value="pending">Pending</Option>
                  <Option value="rejected">Rejected</Option>
                  <Option value="canceled">Canceled</Option>
                  <Option value="open">Open</Option>
                </Select>
              </Form.Item>
            </Form>
          </Modal>

          <Modal
            title="Cancel Booking"
            open={isCancelModalVisible} 
            onCancel={() => setIsCancelModalVisible(false)}
            onOk={handleConfirmCancel}
          ></Modal>
        </Content>
      </Layout>
    </Layout>
  );
};

export default RoomBookingList;

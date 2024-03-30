// UserManagement.tsx

import React, { useState, useEffect } from "react";
import { Layout, Button, Table, Card, Modal, Form, Input } from "antd";
import Sidenav from "../sidenav/Sidenav";
import userData from "./userData"; // Import sample user data
import "./usermanagement.css"; // Import CSS for styling
import axios from 'axios'; //


const { Content } = Layout;

const UserManagement: React.FC = () => {

  const [users, setUsers] = useState<any[]>([]);
  const [selectedUser, setSelectedUser] = useState<any>(null);
  const [isAddUserModalVisible, setIsAddUserModalVisible] = useState<boolean>(false);
  const [isViewModalVisible, setIsViewModalVisible] = useState<boolean>(false);
  const [isEditModalVisible, setIsEditModalVisible] = useState<boolean>(false);
  const [isDeleteConfirmationModalVisible, setIsDeleteConfirmationModalVisible] = useState<boolean>(false);
  const [form] = Form.useForm();

  // Fetch user data when the component mounts
  useEffect(() => {
    fetchUserData();
  }, []);

  // Fetch user data logic
  const fetchUserData = async () => {
    try {
      const response = await axios.get('/api/users');
      setUsers(response.data);
    } catch (error) {
      console.error('Error fetching user data:', error);
    }
  };

  const columns = [
    {
      title: 'ID',
      dataIndex: 'id',
      key: 'id',
      sorter: (a: any, b: any) => a.id - b.id,
    },
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      sorter: (a: any, b: any) => a.name.localeCompare(b.name),
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'Role',
      dataIndex: 'role',
      key: 'role',
    },
    {
      title: 'Department',
      dataIndex: 'department',
      key: 'department',
    },
    {
      title: 'Staff Code',
      dataIndex: 'staffCode',
      key: 'staffCode',
    },
    {
      title: 'Action',
      key: 'action',
      render: (text: any, record: any) => (
        <span>
          <Button type="link" onClick={handleEditUser}>Edit</Button>
          <Button type="link" onClick={handleDeleteUser}>Delete</Button>
        </span>
      ),
    },
  ];

  const handleRowClick = (record: any) => {
    setSelectedUser(record);
    setIsViewModalVisible(true);
    form.setFieldsValue(record); // Populate form fields with user data
  };

  const handleExportData = () => {
    
  };

  const handleCloseViewModal = () => {
    setIsViewModalVisible(false);
  };

  
  const handleCloseEditModal = () => {
    setIsEditModalVisible(false);
  };

  const handleEditUser = () => {
    setIsEditModalVisible(true);
  };

  const handleSaveEdit = async () => {
    try {
      await axios.put(`/api/users/${selectedUser.id}`, form.getFieldsValue());
      fetchUserData(); 
      setIsEditModalVisible(false); 
    } catch (error) {
      console.error('Error saving user edit:', error);
    }
  };

// Delete user logic
  const handleDeleteUser = async () => {
    try {
      await axios.delete(`/api/users/${selectedUser.id}`);
      fetchUserData(); 
      setIsDeleteConfirmationModalVisible(false); 
    } catch (error) {
      console.error('Error deleting user:', error);
    }
  };

  const handleConfirmDeleteUser = () => {
    const updatedUserData = userData.filter(
      (user: any) => user.id !== selectedUser.id
    );

    console.log(updatedUserData);

    setIsDeleteConfirmationModalVisible(false);
    setIsViewModalVisible(false);
  };


  const handleAddUserModal = () => {
    setIsAddUserModalVisible(true);
  };

  const handleCancelAddUserModal = () => {
    setIsAddUserModalVisible(false);
  };

  const handleAddUser = async () => {
    try {
      await axios.post('/api/users', form.getFieldsValue());
      fetchUserData();
      setIsAddUserModalVisible(false); 
    } catch (error) {
      console.error('Error adding user:', error);
    }
  };

  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Layout className="site-layout">
        <Content style={{ margin: "16px" }}>
          <div
            className="site-layout-background"
            style={{ padding: 24, minHeight: 360 }}
          >
            <div className="user-management-header">
              <Button type="primary" onClick={handleExportData}>
                Export Data
              </Button>
              <Button type="primary" onClick={handleAddUserModal}>
                Add User
              </Button>
            </div>
            <Table
              columns={columns}
              dataSource={userData}
              rowKey="id"
              onRow={(record) => ({
                onClick: () => handleRowClick(record),
              })}
            />
            <Modal
              title="Add User"
              visible={isAddUserModalVisible}
              onCancel={handleCancelAddUserModal}
              onOk={handleAddUser}
            >
              <Form form={form}>
                <Form.Item label="Name" name="name">
                  <Input />
                </Form.Item>
                <Form.Item label="Email" name="email">
                  <Input />
                </Form.Item>
                <Form.Item label="Role" name="role">
                  <Input />
                </Form.Item>
                <Form.Item label="Department" name="department">
                  <Input />
                </Form.Item>
                <Form.Item label="Staff Code" name="staffCode">
                  <Input />
                </Form.Item>
              </Form>
            </Modal>
            {selectedUser && (
              <>
                <Modal
                  title="User Information"
                  visible={isViewModalVisible}
                  onCancel={handleCloseViewModal}
                  footer={[
                    <Button key="close" onClick={handleCloseViewModal}>
                      Close
                    </Button>,
                    <Button key="edit" type="primary" onClick={handleEditUser}>
                      Edit
                    </Button>,
                    <Button
                      key="delete"
                      className="delete-button"
                      onClick={() => setIsDeleteConfirmationModalVisible(true)}
                    >
                      Delete
                    </Button>,
                  ]}
                >
                  <p>ID: {selectedUser.id}</p>
                  <p>Name: {selectedUser.name}</p>
                  <p>Email: {selectedUser.email}</p>
                  <p>Role: {selectedUser.role}</p>
                  <p>Department: {selectedUser.department}</p>
                  <p>Staff Code: {selectedUser.staffCode}</p>
                </Modal>
                
                <Modal
                  title="Edit User Information"
                  visible={isEditModalVisible}
                  onCancel={handleCloseEditModal}
                  footer={[
                    <Button key="close" onClick={handleCloseEditModal}>
                      Close
                    </Button>,
                    <Button key="save" type="primary" onClick={handleSaveEdit}>
                      Save
                    </Button>,
                  ]}
                >
                  <Form form={form}>
                    <Form.Item label="ID" name="id">
                      <Input readOnly />
                    </Form.Item>
                    <Form.Item label="Name" name="name">
                      <Input />
                    </Form.Item>
                    <Form.Item label="Email" name="email">
                      <Input />
                    </Form.Item>
                    <Form.Item label="Role" name="role">
                      <Input />
                    </Form.Item>
                    <Form.Item label="Department" name="department">
                      <Input />
                    </Form.Item>
                    <Form.Item label="Staff Code" name="staffCode">
                      <Input />
                    </Form.Item>
                  </Form>
                </Modal>

                <Modal
                  title="Confirm Deletion"
                  visible={isDeleteConfirmationModalVisible}
                  onCancel={() => setIsDeleteConfirmationModalVisible(false)}
                  onOk={handleConfirmDeleteUser}
                >
                  Are you sure you want to delete this user?
                </Modal>
              </>
            )}
          </div>
        </Content>
      </Layout>
    </Layout>
  );
};

export default UserManagement;

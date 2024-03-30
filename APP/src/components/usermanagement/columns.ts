// columns.ts

import React from 'react';
import { Button, Table } from 'antd';

const { Column } = Table;

const columns: Column<any>[] = [
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
        <Button type="link">Edit</Button>
        <Button type="link" onclick={}>Delete</Button>
      </span>
    ),
  },
];

export default columns;

import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Select, Space, Button, Table } from "antd"; // Import Table
import { IRoomDto } from "../../api/Dto";
import {SEARCH_ROOM} from "../../api/apiService";
import {SearchRoom} from "../../api/SearchModel";
import { IRoom } from "../../api/Models";
import "./roombooking.css";

const { Option } = Select;

const RoomBooking: React.FC = () => {
  const [selectedBuilding, setSelectedBuilding] = useState();
  const [selectedFloor, setSelectedFloor] = useState();
  const [selectedRoomType, setselectedRoomType] = useState();
  const [rooms, setRooms] = useState<IRoomDto[]>([]);
  const buildingData = [
    {
        name: 'Lecture Hall',
    },

    {
        name: 'Administration Building',
    },

    {
        name: 'Academic Cluster 1',
    }
  ]

  const roomType = {
    1: "Meeting Room",
    2: "Office",
    // Add mappings for other roomTypeIDs
};

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        // Default search (modify this if needed)
        const searchParams: SearchRoom = {
          BuildingGuid: selectedBuilding,
          FloorGuid: selectedFloor,
          RoomTypeID: selectedRoomType,
        };

        const response= await SEARCH_ROOM(searchParams);
        const rooms: IRoomDto = response.data;
        console.log(response);
        console.log(rooms);
        const roomsWithMappedType: IRoom[] = rooms.map((IRoomDto) => ({
          ...IRoomDto, 
          roomTypeID: roomType[IRoomDto.roomTypeID] || "Unknown"
      }));
        
      setRooms(roomsWithMappedType);

      } catch (error) {
        console.error(error);
      }
    };

    fetchRooms(); 
  }, [selectedBuilding, selectedFloor, selectedRoomType]);

  const filteredRooms = rooms;

  const handleBuildingChange = (value: string) => {
    setSelectedBuilding(value);
  };

  const handleFloorChange = (value: string) => {z
    setSelectedFloor(value);
  };

  const handleRoomTypeChange = (value: string) => {
    setselectedRoomType(value);
  };

  const uniqueFloors = [...new Set(rooms.map((room) => room.floor))].map(
    (floor) => String(floor)
  );

  const uniqueTypes = [...new Set(rooms.map((room) => room.roomType))].map(
    (roomType) => String(roomType)
  );

  // const filteredRooms = rooms.filter(
  //   (room) =>
  //     room.building === selectedBuilding &&
  //     (selectedFloor === "All" || room.floor === Number(selectedFloor)) &&
  //     (selectedRoomType === "All" || room.roomType === selectedRoomType)
  // );

  const navigate = useNavigate();

 
  const handleRoomClick = (guid: string) => {
    console.log("id:", guid);
    const selectedRoom = rooms.find(room => room.guid === guid); 
    if (selectedRoom) {
      navigate(`/rooms/${guid}`, { state: { room: selectedRoom } }); // Pass selectedRoom
  } else {
    // Replace with Room Not Found page in the future
      console.error("Room not found");
  } 
};

  const columns = [
    { title: "No", dataIndex: "id", key: "id" },
    { title: "Room Number", dataIndex: "roomNum", key: "roomNum" },
    { title: "Floor", dataIndex: "floorNum", key: "floorNum" },
    { title: "Building", dataIndex: "buildingName", key: "buildingName" },
    { title: "Room Type", dataIndex: "roomTypeID", key: "roomTypeID" },
    { title: "Capacity", dataIndex: "capacity", key: "capacity" },
    {
      title: "Info Page",
      key: "action",
      render: (room) => (
        <span onClick={() => {
          console.log(room);
          handleRoomClick(room.guid)}}>View More</span>
      ),  
    },
  ];

  return (
    <div className="room-display-container">
      <h2 className="room-booking-title">Room Booking</h2>
      <p>Select the information you want for the room</p>
      <Space className="room-booking-filter" direction="vertical" size="middle">
        {/* Building Filter */}
        <Select
          defaultValue="Lecture Hall"
          style={{ width: 200 }}
          onChange={handleBuildingChange}
        >
          {buildingData.map((building) => (
            <Option key={building.name} value={building.name}>
              {building.name}
            </Option>
          ))}
        </Select>
        {/* Floor filter */}
        <Select
          defaultValue="All"
          style={{ width: 200 }}
          onChange={handleFloorChange}
        >
          <Option value="All">All Floors</Option>
          {uniqueFloors.map((floor) => (
            <Option key={floor} value={floor}>
              {floor}
            </Option>
          ))}
        </Select>
        {/* Room Type filter */}
        <Select
          defaultValue="All"
          style={{ width: 200 }}
          onChange={handleRoomTypeChange}
        >
          <Option value="All">All Types</Option>
          {uniqueTypes.map((roomType) => (
            <Option key={roomType} value={roomType}>
              {roomType}
            </Option>
          ))}
        </Select>
      </Space>

      <h3>List of {selectedBuilding}'s rooms:</h3>
      <Table
        columns={columns}
        dataSource={filteredRooms}
      
      />
    </div>
  );
};

export default RoomBooking;

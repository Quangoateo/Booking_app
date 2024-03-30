import React, { useState, useEffect } from "react";
import { useNavigate, useParams, useLocation } from "react-router-dom";
import { IRoom } from "../../api/Models";
import { IRoomDto } from "../../api/Dto"; // Assuming Models file with IRoom interface
import { SEARCH_ROOM } from "../../api/apiService";
import { SearchRoom } from "../../api/SearchModel";
import "./roominfo.css";
import { Table, Button } from "antd";
import RoomImg from "../../assets/img/room-img.png";
import Pagination from '../pagination/Pagination';

const RoomInfo: React.FC = () => {
  const { roomGuid } = useParams<{ roomGuid: string }>();

  const [selectedOption, setSelectedOption] = useState<string>("Overall Info");
  const navigate = useNavigate();
  const { state } = useLocation();
  const [room, setRoom] = useState<IRoomDto | null>(null);

  const roomTypeMapping = {
    1: "Meeting Room",
    2: "Office",
    // Add mappings for other roomTypeIDs
  };

  useEffect(() => {
    // const fetchRoom = async (guid:string) => {
    //   try {
    //     const searchParams: SearchRoom = { Guid: guid };
    //     const response= await SEARCH_ROOM(searchParams);
    //     const room: IRoomDto = response.data;
    //     console.log(response);
    //     console.log(room);
    //     setRoom(room);
    //   } catch (error) {
    //     console.error("Error fetching room information:", error);
    //   }
    // };

    if (state && state.room) {
      setRoom(state.room); // Get room directly from state
      console.log("Room information fetched from state:", state.room);
    } else {
      // Handle case where the user navigates directly (optional)
      // const { guid } = useParams<{ guid: string }>();
      const guid = state.room.guid;
      fetchRoom(guid);
      console.error("Error fetching room information:", error);
    }
  });

  const facilityColumns = [
    { title: "No", dataIndex: "id", key: "facilty.id" },
    { title: "Item", dataIndex: "name", key: "facilty.name" },
    { title: "Brand", dataIndex: "brand", key: "facilty.brand" },
    { title: "Quantity", dataIndex: "number", key: "facilty.number" },
    { title: "Date Added", dataIndex: "createDate", key: "facilty.createDate"  },
    {
      title: "Actions",
      key: "actions", 
      width: 200, // Adjust width as needed
      render:  (facility: FacilityDto) => ( // `facility` represents a single row object
          <> 
          <div className="room-info__facility-buttons">
              <Button type="primary" size="small" onClick={() => handleEditFacilityClick(facility.id)}>
                  Edit
              </Button>
              <Button danger size="small" onClick={() => handleDeleteFacilityClick(facility.id)}>
                  Delete
              </Button>
          </div>
  
          </>
      ),
  },
  ];

  

  // Switch between Overall Info and More Info tab
  const handleOptionClick = (option: string) => {
    setSelectedOption(option);
  };

  // Return to room booking page
  const handleReturnButtonClick = (option: string) => {
    navigate(`/room-booking/`);
  };

  // Edit facility on click
  const handleEditFacilityClick = (guid: number) => {

  }
  // Delete facility on click
  const handleDeleteFacilityClick = (guid: number) => {

  }

  // Go to create booking
  const handleBookingButtonClick = (roomGuid: number) => {
    navigate(`/rooms/${roomGuid}/create-booking`);
  };

  return (
    <div className="room-info-container">
      {room ? (
        <>
          <img src={RoomImg} alt="Room" className="room-image" />

          <div className="button-and-title-container">
            <button
              className="button return-button"
              type="button"
              onClick={handleReturnButtonClick}
            >
              <i className="bx bx-left-arrow-alt return-button-icon"></i>
              <p>Return</p>
            </button>
            <div className="room-info-title">
              <h2>
                {room.buildingName} - Room {room.roomNum}
              </h2>
            </div>
            <div className="room-info-pseudo">ÁDEFRGTHYJUKL</div>
          </div>

          <div className="room-description">
            <p>{room.description}</p>
          </div>

          {/* Options tab */}
          <div className="option-buttons">
            <button
              className={`option-button ${
                selectedOption === "Overall Info" ? "selected" : ""
              }`}
              onClick={() => handleOptionClick("Overall Info")}
            >
              Overall Info
            </button>
            <button
              className={`option-button ${
                selectedOption === "More Info" ? "selected" : ""
              }`}
              onClick={() => handleOptionClick("More Info")}
            >
              More Info
            </button>
          </div>

          {/* Tabs content */}
          {selectedOption === "Overall Info" && (
            <div className="selected-option-content">
              <div className="info-list-wrapper">
                <ul className="info-list">
                  <li className="info-list-item">
                    <i className="bx bxs-user info-list-item-icon"></i>
                    <div className="info-list-item-info">
                      Capacity: {room.capacity} persons
                    </div>
                  </li>
                  <li className="info-list-item">
                    <i className="bx bxs-home-smile info-list-item-icon"></i>
                    <div className="info-list-item-info">
                      Room type: {room.roomTypeID}
                    </div>
                  </li>
                  <li className="info-list-item">
                    <i className="bx bxs-buildings info-list-item-icon"></i>
                    <div className="info-list-item-info">
                      {room.buildingName} Building
                    </div>
                  </li>
                  <li className="info-list-item">
                    <i className="bx bxs-building info-list-item-icon"></i>
                    <div className="info-list-item-info">
                      Floor {room.floorNum}
                    </div>
                  </li>
                </ul>
              </div>
            </div>
          )}
          {selectedOption === "More Info" && (
            <div className="selected-option-content">
              <div className="room-info__facility-title">List of facilities</div>
              <Table
                columns={facilityColumns}
                dataSource={room.facilities}
                pagination={{  defaultPageSize: 10, showSizeChanger: true, pageSizeOptions: ['10', '20', '30']}}
              />
            </div>
          )}
          <div className="booking-button-container">
            <button
              className="button booking-button"
              type="button"
              onClick={handleBookingButtonClick}
            >
              Create Booking
            </button>
          </div>
        </>
      ) : (
        <div>Loading room information...</div>
      )}
    </div>
  );
};

export default RoomInfo;

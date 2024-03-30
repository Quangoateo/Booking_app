import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { roomData } from "../roombooking/roomData";
import "./createbooking.css";
import { Input, DatePicker, TimePicker, Modal, Button } from "antd";
import { CloseOutlined } from '@ant-design/icons';
import { toast, ToastContainer } from "react-toastify";
import moment from "moment";

interface Booking {
  roomId: number;
  reason: string;
  notes: string;
  startTime: string;
  endTime: string;
}

const CreateBooking = () => {
  const { roomId } = useParams<{ roomId: string }>(); // Get room ID from URL params
  const navigate = useNavigate();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [isCancelModalVisible, setIsCancelModalVisible] = useState(false);
  const [selectedRoom, setSelectedRoom] = useState<RoomData | null>(null);
  const [bookingDate, setBookingDate] = useState(null);
  const [startTime, setStartTime] = useState(moment());
  const [endTime, setEndTime] = useState(moment());
  const [bookingInfo, setBookingInfo] = useState<Booking>({
    roomId: Number(roomId), // Convert string roomId to number
    reason: "",
    notes: "",
    startTime: "",
    endTime: "",
  });

  const handleDateChange = (date, dateString) => {
    setBookingDate(date);
    setBookingInfo({ ...bookingInfo, date: dateString }); // Assuming you add 'date' to the Booking interface
  };
  const handleStartTimeChange = (time, timeString) => {
    setStartTime(time);
    setBookingInfo({ ...bookingInfo, startTime: timeString });
  };
  const handleEndTimeChange = (time, timeString) => {
    setEndTime(time);
    setBookingInfo({ ...bookingInfo, endTime: timeString });
  };

  // Confirm booking logic
  const showModal = () => {
    setIsModalVisible(true);
  };
  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    showModal(); // Show the confirmation modal
  };
  const created_booking = () =>
    toast.success("You have successfully created a booking!", {
      position: "top-right",
      autoClose: 3000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
      theme: "light",
    });
  const handleOk = () => {
    setIsModalVisible(false);
    // Actual booking creation logic (replace placeholder comment)
    console.log("Booking confirmed:", bookingInfo);
    created_booking();
    setTimeout(() => {
      navigate("/room-booking-list");
    }, 4000);
  };
  const handleCancel = () => {
    setIsModalVisible(false);
  };

  // Cancel Booking logic
  const showCancelModal = () => {
    setIsCancelModalVisible(true);
  };
  const handleCancelBooking= (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    showCancelModal(); // Show the confirmation modal
  };
  const handleConfirmCancel = () => {
    setIsCancelModalVisible(false);
    console.log("Booking Canceled:");
      navigate("/room-booking");
  };
  const handleCancelCancel = () => {
    setIsCancelModalVisible(false);
  };

  // Fetch room data on component mount
  useEffect(() => {
    const room = roomData.find((room) => room.id === Number(roomId));
    setSelectedRoom(room);
  }, [roomId]);

  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setBookingInfo({ ...bookingInfo, [event.target.name]: event.target.value });
  };

  return (
    <div className="create-booking-container">
      <div className="create-booking__title-and-button-container">
        <Button 
        type="primary" 
        className="create-booking__close-button" 
        icon={<CloseOutlined/>} 
        onClick={handleCancelBooking}
        />
        <h2 className="create-booking__title">Create Booking</h2>
        <div className="create-booking__emptyobj">VGU</div>
      </div>
      {selectedRoom ? (
        <>
          <form onSubmit={handleSubmit}>
            <div className="create-booking-info">
              <h3 className="create-booking-info-title">Room Information</h3>
              <ul className="create-booking-info-list">
                <li className="create-booking-info-item">
                  Room: {selectedRoom.roomNumber}
                </li>
                <li className="create-booking-info-item">
                  Floor: {selectedRoom.floor}
                </li>
                <li className="create-booking-info-item">
                  Building: {selectedRoom.building}
                </li>
                <li className="create-booking-info-item">
                  {" "}
                  Room type: {selectedRoom.roomType}
                </li>
                <li className="create-booking-info-item">
                  Capacity: {selectedRoom.capacity}
                </li>
              </ul>
            </div>
            <div className="create-booking-form">
              <h3 className="create-booking-form__title">Booking Details</h3>
              <p className="create-booking-form__description">
                Please fill out the information below
              </p>
              <label htmlFor="reason" className="create-booking-form__reason">
                Booking Reason:
              </label>
              <Input.TextArea
                id="reason"
                name="reason"
                className="create-booking-form__reason-input"
                value={bookingInfo.reason}
                onChange={handleChange}
                required
              />
              <label htmlFor="note" className="create-booking-form__note">
                Additional notes:
              </label>
              <Input.TextArea
                className="create-booking-form__note-input"
                id="notes"
                name="notes"
                value={bookingInfo.notes}
                onChange={handleChange}
                placeholder="optional"
              />

              <div className="create-booking-form__time-selection">
                <label htmlFor="date" className="create-booking-form__date">
                  Date:
                </label>
                <DatePicker
                  className="create-booking-form__date-input"
                  onChange={handleDateChange}
                  value={bookingDate}
                />
                <label
                  htmlFor="startTime"
                  className="create-booking-form__start-time"
                >
                  Start Time:
                </label>
                <TimePicker
                  className="create-booking-form__start-time-input"
                  onChange={handleStartTimeChange}
                  value={startTime}
                  format="HH:mm" // Receive only hour and minute
                />
                <label
                  htmlFor="endTime"
                  className="create-booking-form__end-time"
                >
                  End Time:
                </label>
                <TimePicker
                  className="create-booking-form__end-time-input"
                  onChange={handleEndTimeChange}
                  value={endTime}
                  format="HH:mm"
                />
              </div>

              <div className="create-booking__button-container">
                <button
                  className="button create-booking__button"
                  type="button"
                  onClick={handleSubmit}
                >
                  Create Booking
                </button>
              </div>
            </div>
          </form>
          <Modal
            title="Confirm Booking Details"
            visible={isModalVisible}
            onOk={handleOk}
            onCancel={handleCancel}
          >
            <p>Room: {selectedRoom?.roomNumber}</p>
            <p>Date: {bookingDate?.format("YYYY-MM-DD")}</p>{" "}
            {/* Assuming moment object */}
            <p>Start Time: {startTime.format("HH:mm")}</p>
            <p>End Time: {endTime.format("HH:mm")}</p>
            <p>Reason: {bookingInfo.reason}</p>
            <p>Notes: {bookingInfo.notes}</p>
          </Modal>
          <Modal
            title="Cancel Booking"
            visible={isCancelModalVisible}
            okText="Yes" 
            cancelText="No"
            onOk={handleConfirmCancel}
            okButtonProps={{ style: { backgroundColor: 'var(--title-color)', borderColor: 'var(--title-color)'}}}
            onCancel={handleCancelCancel}
            cancelButtonProps={{ style: {color: 'var(--title-color)', borderColor: 'var(--title-color)' }}}
          >
            <p className="">Are you sure you want to cancel your booking procedure?</p>
          </Modal>  
          <ToastContainer />
        </>
      ) : (
        <p>Loading room information...</p>
      )}
    </div>
  );
};

export default CreateBooking;

// VehicleBooking.tsx

import React, { useRef, useEffect, useState } from "react";
import mapboxgl from "mapbox-gl";
import "./VehicleBooking.css";
import DatePicker from "react-datepicker"; // Import the DatePicker component
import "react-datepicker/dist/react-datepicker.css"; // Import the styles for the DatePicker
const VehicleBooking: React.FC = () => {
  const mapContainerRef = useRef<HTMLDivElement>(null);
  const [destination, setDestination] = useState<string>("");
  const [selectedDate, setSelectedDate] = useState<Date | null>(null); // State for selected date
  const [pickupTime, setPickupTime] = useState<Date | null>(null);
  const [vehicleType, setVehicleType] = useState<string>("");
  const [pickupLocation, setPickupLocation] = useState<string>("");
  useEffect(() => {
    if (!mapContainerRef.current) return;

    mapboxgl.accessToken = "pk.eyJ1IjoicXVhbmdkZXB6YWl2dCIsImEiOiJjbHU1cXd1aGUxdzBmMmptd3d2YmxpdzZ1In0.8NZa4eExvVFELoNej_7TKw";

    const map = new mapboxgl.Map({
      container: mapContainerRef.current,
      style: "mapbox://styles/mapbox/streets-v11", // choose your map style here
      center: [-74.5, 40], // starting position [lng, lat]
      zoom: 9 // starting zoom
    });

    // Clean up on unmount
    return () => map.remove();
  }, []);

  const autoExpandInput = (event: React.ChangeEvent<HTMLInputElement>) => {
    const input = event.target;
    input.style.width = 'auto';
    input.style.width = input.scrollWidth + 'px';
  };
  const handleDestinationChange = (event: any) => {
    setDestination(event.target.value);
    // Update the map based on the selected destination
    // You can add your logic here to update the map accordingly
  };
  const handlePickupLocationChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPickupLocation(event.target.value);
  };
  const handleDateChange = (date: Date | null) => {
    setSelectedDate(date);
  };
  const handlePickupTimeChange = (time: Date | null) => {
    setPickupTime(time);
  };
  const getCurrentDate = () => {
    const currentDate = new Date();
    return currentDate;
  };
  const handleVehicleTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setVehicleType(event.target.value);
  };
  const handleConfirm = () => {
    if (selectedDate && pickupTime && vehicleType && destination && pickupLocation) {
      alert(`Booking confirmed for ${selectedDate.toLocaleDateString()} at ${pickupTime.toLocaleTimeString()} from ${pickupLocation} to ${destination} for ${vehicleType}`);
    } else {
      alert("Please fill in all fields.");
    }
  };




  return (
    <div className="vehicle-booking-container">
      <div className="vehicle-booking-left">
        <h1>Vehicle Booking</h1>
        <div className="booking-reason">
          <label htmlFor="BookingReason">Booking Reason</label>
          <input type="text" id="BookingReason" onChange={autoExpandInput} />
        </div>
        <div className="destination">
          <label htmlFor="Destination">Select Destination</label>
          <input type="text" id="Destination" value={destination} onChange={handleDestinationChange} />
        </div>
      </div>
      <div className="vehicle-booking-right">
        <div className="date-time-container">
          <div className="date-picker-container">
            <label htmlFor="DatePicker">Select Dates</label>
            <DatePicker
              selected={selectedDate}
              onChange={handleDateChange}
              dateFormat="MM/dd/yyyy"
              placeholderText={getCurrentDate().toLocaleDateString()}
            />
          </div>
          <div className="time-picker-container">
            <label htmlFor="TimePicker">Pick-up time</label>
            <DatePicker
              selected={pickupTime}
              onChange={handlePickupTimeChange}
              showTimeSelect
              showTimeSelectOnly
              timeIntervals={15}
              timeCaption="Time"
              dateFormat="h:mm aa"
              placeholderText="Select Pickup Time"
            />
          </div>
        </div>
        <div className="pickup-location">
          <label htmlFor="PickupLocation">Pick Up Location</label>
          <input type="text" id="PickupLocation" value={pickupLocation} onChange={handlePickupLocationChange} />
        </div>
        <div className="vehicle-type-container">
          <label htmlFor="VehicleType">Select Vehicle Type</label>
          <select id="VehicleType" value={vehicleType} onChange={handleVehicleTypeChange}>
            <option value="">Select Vehicle Type</option>
            <option value="Bus 32 Seats">Bus 32 Seats</option>
            <option value="Sedan">Sedan</option>
            <option value="SUV">SUV</option>
          </select>
        </div>
      </div>
      <div className="confirm-button-container">
        <button className="orangeButton" onClick={handleConfirm}>Confirm</button>
      </div>
      <div className="map-container" ref={mapContainerRef}></div>
    </div>
  );  
};
export default VehicleBooking;
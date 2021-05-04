import React, { useState, useRef } from 'react';
import axios from 'axios';
import GooglePlacesAutocomplete from 'react-google-places-autocomplete';

const SERVER_URL = process.env.REACT_APP_SERVER_URL;
const GOOGLE_MAPS_API_KEY = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;

const NavBar = (props) => {
  const [searchValue, setSearchValue] = useState(null);
  const [location, setLocation] = useState({});
  const googlePlacesRef = useRef();

  function getGeoCode(locationName) {
    axios
      .post(
        `https://maps.googleapis.com/maps/api/geocode/json?address=${locationName}&key=${GOOGLE_MAPS_API_KEY}`
      )
      .then((response) => {
        let location = {
          name: locationName,
          latitude: response.data.results[0].geometry.location.lat,
          longitude: response.data.results[0].geometry.location.lng,
        };
        setLocation(location);
      });
  }

  function handleSelect(data) {
    setSearchValue(data);
    if (data) {
      getGeoCode(data.label);
    }
  }

  function handleSaveLocationClick() {
    axios.post(`${SERVER_URL}/locations`, location).then((response) => {
      props.loadDashboard();
      //googlePlacesRef.current.value = ''; ???
      setSearchValue(''); //todo: check docs for how to reset/refresh value
    });
  }

  return (
    <div className="navbar">
      <div className="navbar-title">
        <div>Weather Forecaster</div>
      </div>
      <div className="navbar-search">
        <GooglePlacesAutocomplete
          ref={googlePlacesRef}
          apiKey={GOOGLE_MAPS_API_KEY}
          selectProps={{
            searchValue,
            onChange: handleSelect,
            className: 'search',
            isClearable: true,
          }}
          onPress={handleSelect}
        />
        <button
          type="button"
          className="navbar-btn"
          onClick={handleSaveLocationClick}
        >
          <i className="fa fa-plus"></i>
        </button>
        <button
          type="button"
          className="navbar-btn"
          onClick={props.loadDashboard}
        >
          <i className="fa fa-refresh"></i>
        </button>
      </div>
    </div>
  );
};

export default NavBar;

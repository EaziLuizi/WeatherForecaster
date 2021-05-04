import React, { useState, useEffect, Fragment } from 'react';
import Card from './Card';
import axios from 'axios';
import NavBar from './NavBar';

const SERVER_URL = process.env.REACT_APP_SERVER_URL;

const Dashboard = () => {
  const [forecasts, setForecasts] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboard();
  }, []);

  function loadDashboard() {
    setLoading(true);
    axios
      .get(`${SERVER_URL}/locations`)
      .then((response) => {
        setForecasts(response.data);
        setLoading(false);
      })
      .catch((err) => {
        setLoading(false);
      });
  }

  function DeleteLocation(locationId) {
    axios.delete(`${SERVER_URL}/locations/${locationId}`).then((response) => {
      loadDashboard();
    });
  }

  return (
    <Fragment>
      <NavBar loadDashboard={loadDashboard} />

      <div className="dashboard-container">
        {loading ? (
          <h1>Loading...</h1>
        ) : (
          forecasts.map((t, idx) => (
            <Card key={idx} content={t} handleDelete={DeleteLocation} />
          ))
        )}
      </div>
    </Fragment>
  );
};

export default Dashboard;

import React from 'react';

const Card = (props) => {
  function getIconUrl(icon) {
    return `http://openweathermap.org/img/w/${icon}.png`;
  }

  function getDayOfWeek(index) {
    switch (index) {
      case 0:
        return 'Sun';
      case 1:
        return 'Mon';
      case 2:
        return 'Tue';
      case 3:
        return 'Wed';
      case 4:
        return 'Thu';
      case 5:
        return 'Fri';
      case 6:
        return 'Sat';
      default:
        return '';
    }
  }

  function getCurrentTemp(temps) {
    if (temps && temps.length > 0) return temps[0].feelsLike;
    return '';
  }

  return (
    <div className="card">
      <div className="card-header">
        <div className="card-title">
          <h2>{props.content.name}</h2>
        </div>

        <div className="card-highlight">
          {getCurrentTemp(props.content.temperature)}
        </div>

        <div className="card-delete">
          <button onClick={() => props.handleDelete(props.content.id)}>
            X
          </button>
        </div>
      </div>

      <div className="card-body">
        <div className="card-detail">
          {props.content.forecasts.map((t, idx) => {
            let date = new Date(t.dateTime);

            return (
              <div className="card-forecast" key={idx}>
                <div className="card-forecast-title">
                  <div>{getDayOfWeek(date.getDay())}</div>
                  <div>{date.getHours()}:00</div>
                </div>
                <div className="card-forecast-img">
                  <img src={getIconUrl(t.icon)} alt="icon" width="60px" />
                </div>
                <div className="card-forecast-detail">
                  <div>H: {t.max.toFixed(0)}</div>
                  <div>L:{t.min.toFixed(0)}</div>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default Card;

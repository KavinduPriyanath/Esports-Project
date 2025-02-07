import React from "react";
import PropTypes from "prop-types"; 
import "./Card.css";

const Card = ({ image, title, price, description }) => {
  return (
    <div className="card">
      <img src={image} alt={title} />
      <div className="detail">
        <h2>{title}</h2>
        <p>{price}$</p>
      </div>
      <p className="info">{description}</p>
    </div>
  );
};

Card.propTypes = {
  image: PropTypes.string.isRequired, 
  title: PropTypes.string.isRequired, 
  price: PropTypes.number.isRequired, 
  description: PropTypes.string, 
};

export default Card;

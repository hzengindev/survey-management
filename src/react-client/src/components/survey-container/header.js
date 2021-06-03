import React from "react";

const Header = ({ name, description, imageURL }) => {
  
  let headerStyle = {};
  if (imageURL) headerStyle.backgroundImage = `url("${imageURL}")`;

  return (
    <div className="header" style={headerStyle}>
      <h3>{name}</h3>
      <span>{description}</span>
    </div>
  );
};

export default Header;

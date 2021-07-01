import * as React from "react";

export interface IHeaderProps {
  name: string;
  description: string;
  imageURL: string;
}

const Header = ({ name, description, imageURL }: IHeaderProps): JSX.Element => {
  let headerStyle = {
    backgroundImage: "",
  };
  if (imageURL) headerStyle.backgroundImage = `url("${imageURL}")`;

  return (
    <div className="header" style={headerStyle}>
      <h3>{name}</h3>
      <span>{description}</span>
    </div>
  );
};

export default Header;

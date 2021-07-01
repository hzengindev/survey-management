import * as React from "react";
import Loader from "./loader";

const PageLoader = (): JSX.Element => {
  return (
    <div className="page-loader">
      <Loader />
    </div>
  );
};

export default PageLoader;
import React from "react";

const ActionBox = () => {
  return (
    <div className="action-box">
      <div className="pagination">
      <button className="button pagination-button">Previous</button>
      <button className="button pagination-button">Next</button>
      </div>
      <button className="button complete-button">Complete</button>
    </div>
  )
}

export default ActionBox;
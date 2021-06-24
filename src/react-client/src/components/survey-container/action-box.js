import React from "react";

const ActionBox = ({ pageInfo, onPageChangeHandle, onCompleteHandle }) => {
  return (
    <div className="action-box">
      <div className="pagination">
        {pageInfo.currentPage !== 0 && (
          <button
            className="button pagination-button"
            onClick={() => onPageChangeHandle(pageInfo.currentPage - 1)}
          >
            Previous
          </button>
        )}
        {pageInfo.currentPage !== pageInfo.pageCount - 1 && (
          <button
            className="button pagination-button"
            onClick={() => onPageChangeHandle(pageInfo.currentPage + 1)}
          >
            Next
          </button>
        )}
      </div>
      {pageInfo.currentPage === pageInfo.pageCount - 1 && (
        <button className="button complete-button" onClick={() => {onCompleteHandle()}}>
          Complete
        </button>
      )}
    </div>
  );
};

export default ActionBox;

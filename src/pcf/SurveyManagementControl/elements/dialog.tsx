import * as React from "react";

export interface IDialogProps {
  header: string;
  content: string;
  onCloseHandler: () => void;
}

const Dialog = ({
  header,
  content,
  onCloseHandler,
}: IDialogProps): JSX.Element => {
  return (
    <div className="dialog">
      <div className="dialog-box">
        {header && <div className="dialog-header">{header}</div>}
        <div className="dialog-content">{content}</div>

        <button className="dialog-close-button" onClick={onCloseHandler}>
          Close
        </button>
      </div>
    </div>
  );
};

export default Dialog;

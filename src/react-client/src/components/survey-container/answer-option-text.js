import React from "react";

const AnswerOptionText = ({question}) => {
  return (
    <div className="answer-option">
      <div className="answer-option-text">
        <textarea rows="3" onChange={(e) => question.answer = e.target.value} >{question?.answer}</textarea>
      </div>
    </div>
  )
}

export default AnswerOptionText;
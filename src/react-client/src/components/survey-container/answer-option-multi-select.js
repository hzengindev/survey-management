import React from "react";

const AnswerOptionMultiSelect = ({question, answerOptions}) => {
  return (
    <div className="answer-option">
      {answerOptions &&
        answerOptions.map((option) => {
          return <div className="answer-option-multi-select" key={option.id}>
            <label>
              <input type="checkbox" name={`question-${question.order}`} />
              {option.name}
            </label>
          </div>;
        })}
    </div>
  )
}

export default AnswerOptionMultiSelect;
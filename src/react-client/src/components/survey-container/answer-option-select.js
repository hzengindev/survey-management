import React from "react";

const AnswerOptionSelect = ({ question, answerOptions }) => {
  return (
    <div className="answer-option">
      {answerOptions &&
        answerOptions.map((option) => {
          return <div className="answer-option-select" key={option.id}>
            <label>
              <input type="radio" name={`question-${question.order}`} />
              {option.name}
            </label>
          </div>;
        })}
    </div>
  );
};

export default AnswerOptionSelect;

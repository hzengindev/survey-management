import React, { useContext, useState } from "react";
import { AnswerContext } from "../../contexts/answer-context";

const AnswerOptionMultiSelect = ({ question, answerOptions }) => {
  const [selectedOptions, setSelectedOptions] = useState([]);
  const context = useContext(AnswerContext);

  const onAnswerSelect = (e, option) => {
    let _selectedOptions = [];
    if (e.target.checked) {
      _selectedOptions = [...selectedOptions, option];
    } else {
      _selectedOptions = [...selectedOptions.filter((z) => z.id !== option.id)];
    }
    setSelectedOptions(_selectedOptions);

    context.saveAnswer(question, _selectedOptions);
  };

  const answer = context.answers.find(
    (z) => z.questionId === question.id
  )?.multiSelectAnswer;

  return (
    <div className="answer-option">
      {answerOptions &&
        answerOptions.map((option) => {
          return (
            <div className="answer-option-multi-select" key={option.id}>
              <label>
                <input
                  type="checkbox"
                  name={`question-${question.order}`}
                  onChange={(e) => onAnswerSelect(e, option)}
                  defaultChecked={answer && answer.some((z) => z === option.id)}
                />
                {option.name}
              </label>
            </div>
          );
        })}
    </div>
  );
};

export default AnswerOptionMultiSelect;

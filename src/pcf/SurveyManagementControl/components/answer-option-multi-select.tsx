import * as React from "react";
import { useContext, useState } from "react";
import { AnswerContext } from "./../contexts/answer-context";

const AnswerOptionMultiSelect = ({
  question,
  answerOptions,
}: any): JSX.Element => {
  const [selectedOptions, setSelectedOptions] = useState([]);
  const context: any = useContext(AnswerContext);

  const onAnswerSelect = (
    e: React.ChangeEvent<HTMLInputElement>,
    option: any
  ) => {
    let _selectedOptions: any = [];
    if (e.target.checked) {
      _selectedOptions = [...selectedOptions, option];
    } else {
      _selectedOptions = [
        ...selectedOptions.filter((z: any) => z.id !== option.id),
      ];
    }
    setSelectedOptions(_selectedOptions);

    context.saveAnswer(question, _selectedOptions);
  };

  const answer = context.answers.find(
    (z: any) => z.questionId === question.id
  )?.multiSelectAnswer;

  return (
    <div className="answer-option">
      {answerOptions &&
        answerOptions.map((option: any) => {
          return (
            <div className="answer-option-multi-select" key={option.id}>
              <label>
                <input
                  type="checkbox"
                  name={`question-${question.order}`}
                  onChange={(e) => onAnswerSelect(e, option)}
                  defaultChecked={
                    answer && answer.some((z: any) => z === option.id)
                  }
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

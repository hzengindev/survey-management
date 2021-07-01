import * as React from "react";
import { useContext } from "react";
import { AnswerContext } from "./../contexts/answer-context";

const AnswerOptionText = ({ question }: any) => {
  const context: any = useContext(AnswerContext);

  const onAnswerEnter = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    context.saveAnswer(question, e.target.value);
  };

  const answer = context.answers.find(
    (z: any) => z.questionId === question.id
  )?.textAnswer;

  return (
    <div className="answer-option">
      <div className="answer-option-text">
        <textarea
          value={answer}
          rows={3}
          onChange={(e) => onAnswerEnter(e)}
        ></textarea>
      </div>
    </div>
  );
};

export default AnswerOptionText;

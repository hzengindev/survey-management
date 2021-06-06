import React from "react";
import Question from "./question";

const Page = ({questions}) => {
  
  return (
    <div className="page">
      {questions && questions.map((item) => <Question key={item.id} question={item} />)}
    </div>
  );
};

export default Page;

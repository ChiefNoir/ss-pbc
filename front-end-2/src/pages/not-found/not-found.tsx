import React, { useMemo } from 'react';
import './not-found.scss';

// function NotFound() {
//   console.log("NotFound");
  
//   return (
//   <div className="container">
//     <div className="neon">
//       <div className="blink3s">pa</div>ge&nbsp;<div className="blink5s">no</div>t&nbsp;found</div>
//     </div>
//   );
// }

class NotFound extends React.Component {

   constructor(props: any)
  {
    console.log("constructor: NotFound");

     super(props);
   }

  shouldComponentUpdate() {
    console.log("shouldComponentUpdate: NotFound");
    return false;
  }

  render() {
    console.log("RENDER: NotFound");
    return (
      <div className="container">
     <div className="neon">
       <div className="blink3s">pa</div>ge&nbsp;<div className="blink5s">no</div>t&nbsp;found</div>
    </div>
    );

    
  }


}

export default NotFound;
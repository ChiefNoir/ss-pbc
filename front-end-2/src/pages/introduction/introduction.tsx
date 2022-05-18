import React, { useState, useEffect }  from 'react';
import logo from '../../../src/logo.svg';
import './introduction.scss';
import Navigation from '../../features/ui/navigation/navigation';

function Introduction() {

  let introduction;
let api = process.env.REACT_APP_API_PUBLIC_ENDPOINT;
console.assert(api);
  fetch(api+'/introduction')
      .then((res) => introduction = res.json())
      ;

      const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [users, setUsers] = useState([]);


  return (

    <div className="container">
      <div>
        <h1 className="headline">{ introduction.title } </h1>
        {/* <mat-divider></mat-divider> */}
        <div className="description">{introduction.content}</div>
      </div>
    
    <img className="poster"
       alt="{ introduction.posterDescription}"
       src={(introduction.posterUrl || '/assets/images/placeholder-tall.png')}/>
    </div>

  );
}

export default Introduction;

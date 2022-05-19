import React, { useState, useEffect }  from 'react';
import './introduction.scss';
import { Introduction, ExecutionResult } from '../../models'
import PublicApi from '../../services/PublicApi';

function IntroductionPage() {
  const [introduction, setIntroduction] = useState<Introduction | null>(null);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
      setLoading(true);
      var result = await PublicApi.getIntroduction();

      setIntroduction(result.data.data);
      setLoading(false);
    };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div>
      {loading? 
    <div>loading</div>  
    :
    <div>

      <div>
        <h1 >{ introduction?.title } </h1>
        {/* <mat-divider></mat-divider> */}
        <div dangerouslySetInnerHTML={{__html: introduction?.content || ''}}></div>
      </div>
    
    <img 
      //  src={(introduction?.posterUrl || '/assets/images/placeholder-tall.png')}/>
      src={'/assets/images/placeholder-tall.png'}/>
    </div>
    }
</div>


  );
}

export default IntroductionPage;

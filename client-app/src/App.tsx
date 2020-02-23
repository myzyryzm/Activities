import React, {useState, useEffect} from 'react';
import logo from './logo.svg';
import './App.css';
import {cars} from './demo'
import CarItem from './CarItem'
import axios from 'axios'
import {Header, Icon, List} from 'semantic-ui-react';

interface Value {
  id: number,
  name: string
}

const App: React.FC = () => {
  const [values, setValues] = useState<Value[]>([]);
  
  useEffect(() => {
    axios.get('http://localhost:5000/api/values')
    .then((response) => {
      setValues(response.data);
    })
  }, [])
  
  return (
    <div>
      <Header as='h2'>
        <Icon name='plug' />
        <Header.Content>Uptime Guarantee</Header.Content>
      </Header>
      <List>
        {values.map((value) => {
            return <List.Item key={value.id}>{value.name}</List.Item>
          })}
      </List>
    </div>
  );
}

export default App;

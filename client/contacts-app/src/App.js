import React from 'react';
import './App.css';
import Contacts from './components/Contacts';
import Container from '@material-ui/core/Container';

const App = () => {
  return (
    <div className='App'>
      <header></header>
      <Container maxWidth='lg'>
        <Contacts />
      </Container>
    </div>
  );
};

export default App;

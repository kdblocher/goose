import React, { useEffect, useState } from 'react';
import { getUsers, User } from './api';
import './App.css';
import UserList from './UserList';

function App() {
  let [users, setUsers] = useState<readonly User[]>([])
  useEffect(() => {
    getUsers().then(setUsers)
  }, [])
  return (
    <div className="App">
      <h3>Goose Controller</h3>
      <UserList users={users} />
    </div>
  );
}

export default App;

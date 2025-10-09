import React, { useState } from "react";
import UserList from "./components/UserList";
import AddUser from "./components/AddUser";

function App() {
  const [reload, setReload] = useState(false);

  const handleUserAdded = () => setReload(!reload);

  return (
    <div className="App">
      <h1>Quản lý User</h1>
      <AddUser onUserAdded={handleUserAdded} />
      <UserList key={reload} />
    </div>
  );
}

export default App;

import { useEffect, useState } from "react";
import CardList from "../Components/Card/CardList";
import Search from "../Components/Search/Search";
import "./ToDo.css";
import { ToDoMain } from "../Components/ToDo/ToDoMain";

// function App() {
//   return (
//     <div className="app">
//       <Search />
//       <CardList />
//     </div>
//   );
// }

function App() {
  return <ToDoMain />;
}

export default App;

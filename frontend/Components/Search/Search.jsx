import React, { useState } from "react";

const Search = () => {
  const [search, setSearch] = useState();
  const handleChange = (e) => {
    setSearch(e.value);
    console.log(e.value);
  };
  const onClick = (e) => {
    console.log(e);
  };
  return (
    <div>
      <input value={search} onChange={(e) => handleChange(e)}></input>
      <button onClick={(e) => onClick(e)} />
    </div>
  );
};

export default Search;

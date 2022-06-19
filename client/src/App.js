import axios from "axios";
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import Home from "./Pages/Home/Home";

class App extends React.Component {
  constructor(props){
    super(props);
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
  }

  render() {
    return (
      <div className = "App"> 
        <Router>
          <header></header>
          <Routes>
            <Route path = "/" element = {<Home />}/>
          </Routes>
        </Router>
      </div>
    );
  }
}

export default App;
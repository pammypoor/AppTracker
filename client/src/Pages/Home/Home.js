import React from "react";
import NavigationBar from "../../Components/NavigationBar/NavigationBar";

class Home extends React.PureComponent {
    constructor(props){
        super(props);
    }
    render() {
        return(
            <div className ="home-wrapper">
                {<NavigationBar/>}
                TEST
            </div>
        );
    }
}

export default Home;
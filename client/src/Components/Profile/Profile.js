import React from "react"; 

import "./Profile.css";

class Profile extends React.PureComponent {
    constructor(props){
        super(props); 
    }

    componentDidMount() {
       
    }

    

    render() {
        return(
            <div className ="profile-container">
                <h2>Public profile</h2>
            </div>
        );
    }
}

export default Profile;
import React from "react";
import ProfileForm from "../Forms/ProfileForm/ProfileForm"; 

import "./UserEditableProfile.css";

class UserEditableProfile extends React.PureComponent {
    constructor(props){
        super(props); 
    }

    componentDidMount() {
       
    }

    

    render() {
        return(
            <div className ="profile-container">
                <h2>Public profile</h2>
                <ProfileForm/>
            </div>
        );
    }
}

export default UserEditableProfile;
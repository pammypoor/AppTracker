import React from "react";
import jwt_decode from "jwt-decode";  

import "./Settings.css";
import NavigationBar from "../../Components/NavigationBar/NavigationBar";
import Profile from "../../Components/Profile/Profile.js";

class Settings extends React.PureComponent {
    constructor(props){
        super(props); 
        this.token = sessionStorage.getItem('authorization');
    }

    componentDidMount() {
        this.checkToken();
    }

    checkToken = () => {
        const token = sessionStorage.getItem('authorization');
        if(token) {
            const decoded = jwt_decode(token);
            const tokenExpiration = decoded.tokenExpiration;
            const now = new Date();

            if(now.getTime() > decoded.exp * 1000){
                localStorage.removeItem('authorization');
                window.location = '/';
            }
        } else {
            // Token doesn't exist or not valid
            localStorage.removeItem('authorization');
            
        }
    }

    render() {
        

        return(
            <div className ="settings-wrapper">
                <div className="settings-navbar-wrapper">
                    <NavigationBar/>
                </div>
                <div className="profile-wrapper">
                    <Profile/>
                </div>
            </div>
        );
    }
}

export default Settings;
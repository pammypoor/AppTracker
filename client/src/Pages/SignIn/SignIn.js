import React from "react";
import SignInForm from "../../Components/Forms/SignInForm/SignInForm";

class SignIn extends React.PureComponent {
    render() {
        return (
            <div className="signin-container">
                {<SignInForm/>}
            </div>
        );
    }
}

export default SignIn;
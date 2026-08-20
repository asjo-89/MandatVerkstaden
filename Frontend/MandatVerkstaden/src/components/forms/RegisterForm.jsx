import { ConfirmButton } from "../buttons/ConfirmButton"
import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { Input } from "../inputs/Input"
import { ApiFetch } from "../helpers/ApiFetch"
import API_URL from '../../ApiUrl'
import NormalizeErrors from "../helpers/NormalizeErrors"

const RegisterForm = () => {
  const navigate = useNavigate();

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [matchingPassword, setMatchingPassword] = useState("");
  const [email, setEmail] = useState("");

  const [modelErrors, setModelErrors] = useState({});
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);

  const normalizeErrors = NormalizeErrors;
  
  // const normalizeErrors = (errors) => {
  //   console.log("Normalizing errors:", errors);
  //   const normalizedErrors = {};
  //   if(errors && typeof errors === "object") {
  //     Object.entries(errors).forEach(([key, message]) => {
  //       normalizedErrors[key.toLowerCase()] = Array.isArray(message) ? message.join(", \n") : message;
  //     })
  //   }
  //   return normalizedErrors;
  // }

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setModelErrors({});
    try {
          await ApiFetch(`${API_URL}/auth/register`, {
          method: "POST",
          body: { firstName, lastName, username, email, password, matchingPassword }
        });

        setSuccess("Registrering lyckades!");
        setTimeout(() => {
          navigate("/login");
        }, 2000);
    }
    catch (err) {
      console.log("Error during registration:", err);
      if(err.errors) {
        setModelErrors(normalizeErrors(err.errors));
      }
      setError(err.message || "Registrering misslyckades. Vänligen försök igen.");
    }
  };


  return (
    <div className="card card-login">
      <h2 className="manrope-semibold text-center">Registrera</h2>
      <form onSubmit={handleSubmit} noValidate>
        <div className="input-group input-group-medium">
          <Input 
            id="firstName" 
            statusClass={`${modelErrors.firstname ? 'input-error' : ''}`}
            name="firstName" 
            htmlFor="firstName" 
            label="Förnamn" 
            placeholder="Ange förnamn" 
            type="text" 
            onChange={(e) => setFirstName(e.target.value)}
            value={firstName} />
          <span className={`input-error-text ${modelErrors.firstname ? 'show-flex' : 'hide'}`}>{modelErrors.firstname}</span>
        </div>
        <div className="input-group input-group-medium">
          <Input 
            id="lastName" 
            statusClass={`${modelErrors.lastname ? 'input-error' : ''}`}
            name="lastName" 
            htmlFor="lastName" 
            label="Efternamn" 
            placeholder="Ange efternamn" 
            type="text" 
            onChange={(e) => setLastName(e.target.value)}
            value={lastName} />
          <span className={`input-error-text ${modelErrors.lastname ? 'show-flex' : 'hide'}`}>{modelErrors.lastname}</span>
        </div>
        <div className="input-group input-group-medium">
          <Input 
            id="username" 
            statusClass={`${modelErrors.username ? 'input-error' : ''}`}
            name="username" 
            htmlFor="username" 
            label="Användarnamn" 
            placeholder="Ange användarnamn" 
            type="text" 
            onChange={(e) => setUsername(e.target.value)}
            value={username} />
          <span className={`input-error-text ${modelErrors.username ? 'show-flex' : 'hide'}`}>{modelErrors.username}</span>
        </div>
        <div className="input-group input-group-medium">
          <Input 
            id="email" 
            statusClass={`${modelErrors.email ? 'input-error' : ''}`}
            name="email" 
            htmlFor="email" 
            label="E-post" 
            placeholder="Ange e-post" 
            type="email" 
            onChange={(e) => setEmail(e.target.value)}
            value={email} />
          <span className={`input-error-text ${modelErrors.email ? 'show-flex' : 'hide'}`}>{modelErrors.email}</span>
        </div>
        <div className="input-group input-group-medium">
          <Input 
            id="password" 
            statusClass={`${modelErrors.password ? 'input-error' : ''}`}
            name="password" 
            htmlFor="password" 
            label="Lösenord" 
            placeholder="Ange lösenord" 
            type="password" 
            onChange={(e) => setPassword(e.target.value)}
            value={password} />
          <span className={`input-error-text ${modelErrors.password ? 'show-flex' : 'hide'}`}>{modelErrors.password}</span>
        </div>   
        <div className="input-group input-group-medium">
          <Input 
            id="matchingPassword" 
            statusClass={`${modelErrors.matchingpassword ? 'input-error' : ''}`}
            name="matchingPassword" 
            htmlFor="matchingPassword" 
            label="Bekräfta lösenord" 
            placeholder="Ange lösenord igen" 
            type="password" 
            onChange={(e) => setMatchingPassword(e.target.value)} 
            value={matchingPassword} />
          <span className={`input-error-text ${modelErrors.matchingpassword ? 'show-flex' : 'hide'}`}>{modelErrors.matchingpassword}</span>
        </div>       

        {error && <span className="alert-danger">{error.message || error}</span>}
        {success && <span className="alert-success">{success.message || success}</span>}

        <ConfirmButton btnType="submit" className="btn-login" btnText="Registrera" />
      </form>
    </div>
  )
}

export default RegisterForm
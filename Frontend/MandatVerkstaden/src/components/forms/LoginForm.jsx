import { ConfirmButton } from "../buttons/ConfirmButton"
import { useState } from "react"
import { useAuth } from "../contexts/AuthContext"
import { useNavigate } from "react-router-dom"
import { Input } from "../inputs/Input"

const LoginForm = () => {
  const {login, error } = useAuth();
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

      const success = await login(username, password);
      if(success) {
        navigate("/dashboard");
        return;
      }
  };

  return (
    <div className="card card-login">
      <h2 className="manrope-semibold text-center">Logga in</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-group input-group-medium">
          <Input 
            id="username" 
            name="username" 
            htmlFor="username" 
            label="Användarnamn" 
            placeholder="Ange användarnamn" 
            type="text" 
            onChange={(e) => setUsername(e.target.value)} 
            value={username} />
        </div>
        <div className="input-group input-group-medium">
          <Input 
            id="password" 
            name="password" 
            htmlFor="password" 
            label="Lösenord" 
            placeholder="Ange lösenord" 
            type="password" 
            onChange={(e) => setPassword(e.target.value)} 
            value={password} />
        </div>        
        {error && <p className="alert-danger">{error.message || error}</p>}
        <ConfirmButton btnType="submit" className="btn-login" btnText="Logga in" />
      </form>
      <div className="login-footer">
        <p>Har du inget konto?</p> 
        <a href="/register">Registrera dig</a>
      </div>
    </div>
  )
}

export default LoginForm
import { useAuth } from "../contexts/AuthContext";
import { Navigate } from "react-router-dom";

export const RootRedirect = () => {

    const { user, loading } = useAuth();

    if(loading) {
        return <div>Loading...</div>
    }

    if(!user) {
        return <Navigate to="/login" replace />
    }
    
    return <Navigate to="/scenario" replace />
}


export const DeleteButton = ({ btnText, onClick, className }) => {
  return (
        <>
            <button type="button" className={`btn btn-delete ${className || ""}`} onClick={onClick}>{btnText || ""}</button>
        </>
  )
}

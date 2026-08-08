
export const ConfirmButton = ({ btnText, onClick, className }) => {
  return (
        <>
            <button className={`btn btn-confirm manrope-semibold ${className || ""}`} onClick={onClick}>{btnText || ""}</button>
        </>
  )
}

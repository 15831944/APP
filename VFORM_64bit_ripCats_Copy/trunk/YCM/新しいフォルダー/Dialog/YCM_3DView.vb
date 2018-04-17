Option Explicit On
'Option Strict On
Imports OpenGLLib
Public Class YCM_3DView
    '----------------------------------------------------------
    '�����f�[�^
    Private mhGL As IntPtr  'OpenGL�n���h��
    Private m_points As New ArrayList
    Dim crl_mode As ControlMode
    Dim blnRotateClick As Boolean
    Dim blnMoveClick As Boolean
    Public ii As Integer
    Private Const OBJECTSIZE As Single = 20         '�I�u�W�F�N�g�̃T�C�Y
    Dim Day As Single = 1
    Dim aa As Single = 200000000
    Dim bb As Single = 0
    Dim cc As Single = 1
    Dim dd As Single = 1
    Dim ee As Single = 1
    'Dim scale As Double = 1
    Dim dblxangle() As Double
    Dim dblyangle() As Double
    'Public blnCtrlKey As Boolean = False
    Public click_P As New GeoPoint
    Dim t As Single
    Dim bln_mouseClick As Boolean
    Private Const ROTATEINTERVAL As Integer = 2000  '��]����(�~���b)
    Private Const EYEX As Double = 0.0      '���_�ʒu
    Private Const EYEY As Double = 20.0
    Private Const EYEZ As Double = 100.0
    'Dim xangle As Double
    'Dim yangle As Double
    Public dblwindowLeft As Double
    Public dblwindowRight As Double
    Public dblwindowBottom As Double
    Public dblwindowTop As Double
    Public Wheel_Speedx As Double
    Public Wheel_Speedy As Double
    Dim Point_Name As UInteger
    Public Camera_Name As UInteger '20170209 baluu edit (Dim -> Public)
    Dim Ray_Name As UInteger
    Dim Label_Name As UInteger
    Dim Drawray_Name As UInteger
    Dim Circle_Name As UInteger
    Dim blnSelectMode As Boolean
    Dim screen_scaleX As Double
    Dim screen_scaleY As Double
    Dim view_UpLow As Double
    Dim Pick_windows_pos1 As New GeoPoint
    Dim Pick_windows_pos2 As New GeoPoint

    Public Model3dPath As String '20170110 baluu add start
    Public ReloadModel3dPath As Boolean = True '20170110 baluu add end
    Public Shared flgExit3DView As Boolean = False
    '20170209 baluu add start
    Public DrawUserLine As Boolean = True
    Public DrawSunpoLine As Boolean = True
    Public DrawSekkeiLine As Boolean = True
    '20170209 baluu add end



    Dim CameraDispFlag As Boolean

    'TestCode kiryu
    Dim m_x As Integer
    Dim m_y As Integer
    Dim befor_m As System.Drawing.Point

    Dim tx As Double
    Dim ty As Double
    Dim tz As Double

    Private Sub YCM_3DView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            crl_mode = ControlMode.Normal
            IOUtil.blnSelectMode = False
            'clear area
            m_blnCommandArea = False
            gppointstart = New GeoPoint
            gppointend = New GeoPoint
            intgppointarea = 0
            If model_SingleSelct = False Then
                model_picks.Clear()
            End If
            'IOUtil.EndThread()
        End If

    End Sub
    '�t�H�[�����[�h��
    Private Sub YCM_3DView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim aaa As New Point
        'aaa.X = 0
        'aaa.Y = 50
        'Me.Location = aaa
        bln_mouseClick = False
        t = 5
        eye_x = 0 : eye_y = 0 : eye_z = -10
        ent_x = 0 : ent_y = 0 : ent_z = 0
        xangle = 0 : yangle = 0
        Dim sx As Integer = Me.ClientSize.Width
        Dim sy As Integer = Me.ClientSize.Height '- 50

        dblwindowBottom = -0.5 : dblwindowTop = 0.5
        dblwindowLeft = -sx * 0.5 / sy : dblwindowRight = sx * 0.5 / sy

        crl_mode = ControlMode.Normal
        blnRotateClick = False
        Wheel_Speedy = 1.0 : Wheel_Speedx = sx / sy
        Point_Name = 1
        Camera_Name = 10000
        Ray_Name = 20000
        Label_Name = 30000
        Drawray_Name = 40000
        Circle_Name = 50000
        blnSelectMode = False
        r_tmp = 100.0 * Math.Cos(Math.PI / (180 / 1))

        pnt_Xais.setXYZ(r_tmp * Math.Sin(Math.PI / (180 / 1)), 0, 100.0)
        pnt_Yais.setXYZ(0, r_tmp * Math.Sin(Math.PI / (180 / 1)), 100.0)

        Vec_Screen_X.setXYZ(1, 0, 0) : Vec_Screen_Y.setXYZ(0, 1, 0)

    End Sub

    '�t�H�[���N���[�Y��
    Private Sub YCM_3DView_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    End Sub
    Public Sub setWndMode(ByVal value As Integer)
        crl_mode = value
    End Sub

    '���C���v���V�[�W��
    Public Sub MainProc()
        Dim a As UInteger = 1
        'OpenGL������
        mhGL = GLHelper.StartOpenGL(Me.Handle)
        SetProjectionAndViewport()

        '�f�o�C�X�R���e�L�X�g�̎擾
        Dim g As Graphics = Me.CreateGraphics
        Dim hDC As IntPtr = g.GetHdc()

        '�`��ݒ�
        ' SetMaterial()

        ' glEnable(GL_DEPTH_TEST) '�[�x�e�X�g��L���i�A�ʏ����j
        glCullFace(GL_BACK)
        glEnable(GL_CULL_FACE) '�w�ʏ���
        glEnable(GL_NORMALIZE)      '�@���x�N�g���̎������K����L��
        'glEnable(GL_COLOR_MATERIAL) '�}�e���A���̐F��L��
        glShadeModel(GL_SMOOTH)

        'glInitNames()
        'glPushName(a)
        'glRenderMode(GL_SELECT)
        'glEnable()
        '�i����ɂ��ʂ̑O�オ�������`�悳��܂��B�j


        '���C�����[�v
        Do While Me.Visible
            'If bindataviewclickmode = True Then
            '    Dim aaaa As String = Data_Point.DGV_DV.Rows(intdataviewdraw).Cells(0).Value.ToString
            'End If

            Day = Day + 1
            '�`��
            Draw()

            '�o�b�t�@�ؑ�
            SwapBuffers(hDC)


            '�G���[�`�F�b�N�i�����j
            GLHelper.ErrorCheck()

            '�t���[�����[�g�X�V
            If GLHelper.UpdateFrameRate() Then
                'Debug.Print("FrameRate = {0}", GLHelper.FrameRate)
            End If

            '���b�Z�[�W����
            DispatcherHelper.DoEvents()
            Try
                If My.Application.MainWindow.IsVisible = False Then
                    ' Me.Visible = False

                    Exit Do

                End If
            Catch ex As Exception
                Exit Do
            End Try
          
            If MainFrm.flgKoushin3DPoint = True Then
                YMC_3DViewReDraw_NoKousin(m_strDataBasePath)
                MainFrm.flgKoushin3DPoint = False
            End If
            'SUSANO ADD START 20160523
            If flgManualScaleSetting = True Then
                MainFrm.BtnToolScaleSet()
                flgManualScaleSetting = False
            End If
            
            'SUSANO ADD END 20160525
            'If flgExit3DView = True Then
            '    Exit Do
            'End If
        Loop

        '�f�o�C�X�R���e�L�X�g�̉��
        g.ReleaseHdc(hDC)
        g.Dispose()

        'OpenGL�I������
        GLHelper.EndOpenGL(mhGL)

        '�t�H�[�������
        Me.Hide()

    End Sub
    '���C�g�̐ݒ�
    Private Sub SetLight(ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single)
        glEnable(GL_LIGHTING)
        glEnable(GL_LIGHT0)

        '�����̐ݒ�

        Dim pos() As Single = {PosX, PosY, PosZ, 0}
        ' Dim pos() As Single = {100, 100, 100, 0.0}
        Dim light_ambient() As Single = {1, 1, 1, 1.0}
        Dim light_diffuse() As Single = {1, 1, 1, 1.0}
        Dim light_specular() As Single = {0.0, 0.0, 0.0, 1.0}

        glLightfv(GL_LIGHT0, GL_POSITION, pos)
        glLightfv(GL_LIGHT0, GL_AMBIENT, light_ambient)
        glLightfv(GL_LIGHT0, GL_DIFFUSE, light_diffuse)
        glLightfv(GL_LIGHT0, GL_SPECULAR, light_specular)


        'glEnable(GL_DEPTH_TEST)
        ''  glDisable(GL_DEPTH_TEST)
        ''�J�����O�F�\�ʂ̂ݕ\��
        'glEnable(GL_CULL_FACE)
        'glCullFace(GL_BACK)


    End Sub
    '�`��
    Private Sub Draw()

        bln_ClickPoint = False : bln_ClickCamera = False : bln_ClickRay = False
        If crl_mode = ControlMode.Rotate Then
            Me.Cursor = Cursors.PanEast
        ElseIf crl_mode = ControlMode.Normal Then
            Me.Cursor = Cursors.Arrow
        ElseIf crl_mode = ControlMode.Move Then
            Me.Cursor = Cursors.Hand
        End If
        Dim tm As Integer
        tm = System.Environment.TickCount And Integer.MaxValue
        tm = tm Mod ROTATEINTERVAL

        glutInitDisplayMode(GLUT_DEPTH)  '�B�ʏ���

        'eye_x = ents_centerC.x : eye_y = ents_centerC.y : eye_z = ents_centerC.z - 100
        glInitNames()
        glPushName(0)
        '�N���A
        glClear(GL_COLOR_BUFFER_BIT)
        glClear(GL_DEPTH_BUFFER_BIT)

        glEnable(GL_DEPTH_TEST)
        '�w�i�F�̐ݒ�
        '    glClearColor(0.1, 0.8, 0.9, 1.0) '���F�`�O���[
        'glClearColor(1.0, 1.0, 1.0, 1.0) '��
        glClearColor(0, 0, 0, 0) '��
        'glClearColor(1.0, 1.0, 0.9, 1.0) '�A�C�{���[
        ''glClearColor(0.95, 0.95, 0.95, 1.0) '�O���[

        '0% �` 100% �̏�Ԃ� float �^�� 0.0 �` 1.0 �ŕ\���܂��B���ׂĂ� 0 �ł���΍��ł���A1 �ł���Δ������
        'glLoadIdentity()

        'ground�ݒ�
        'Dim ground_max_x As Double = 300.0
        'Dim ground_max_y As Double = 300.0
        'Dim ground_max_z As Double = 300.0
        'glBegin(GL_LINES)
        'Dim lx As Double
        ''Dim ly As Double
        'Dim lz As Double
        ''For ly = -600 To 600
        ''    glVertex3d(-ground_max_x, ly, -ground_max_z)
        ''    glVertex3d(ground_max_x, ly, ground_max_z)
        ''Next ly

        'For lx = -600 To 600
        '    glVertex3d(lx, -1, ground_max_z)
        '    glVertex3d(lx, -1, -ground_max_z)
        'Next lx

        'For lz = -600 To 600
        '    glVertex3d(ground_max_x, -1, lz)
        '    glVertex3d(-ground_max_x, -1, lz)
        'Next lz
        'glEnd()


        If Not blnSelectMode Then
            glMatrixMode(GL_PROJECTION)                 '�s�񃂁[�h���ˉe�ϊ��ɂ���
            glLoadIdentity()                            '���݂̍s��ɒP�ʍs��ɂ���
            'If xangle = 0 Then
            '    Dim dbllenX As Double, dblLenY As Double
            '    dbllenX = dblwindowRight - dblwindowLeft
            '    dblLenY = dblwindowTop - dblwindowBottom
            '    dblwindowLeft = ents_centerC.x - dbllenX / 2
            '    dblwindowRight = ents_centerC.x + dbllenX / 2
            '    dblwindowBottom = ents_centerC.y - dblLenY / 2
            '    dblwindowTop = ents_centerC.y + dblLenY / 2
            'End If
            'dblwindowLeft = 
            glOrtho(dblwindowLeft, dblwindowRight, dblwindowBottom, dblwindowTop, pointnear, pointfar)
        End If
        '���f���r���[�s��̐ݒ�
        glMatrixMode(GL_MODELVIEW)                  '�s�񃂁[�h�����f���r���[�ɂ���
        glLoadIdentity()                            '���݂̍s��ɒP�ʍs��ɂ���

        '�ړ����邽�߁A�X�N���[���ɕ\������x�N�g�����v�Z����
        '��]����ꍇ�ŁA�X�N���[���ɕ\������x�N�g�����ύX���܂��̂ŁA
        '���̃x�N�g���𐏎��Ɍv�Z����K�v������B
        xangle_Yais = xangle : yangle_Yais = yangle + 1
        xangle_Xais = xangle + 1 : yangle_Xais = yangle

        screen_scaleX = (dblwindowRight - dblwindowLeft) / Me.ClientSize.Width
        screen_scaleY = (dblwindowTop - dblwindowBottom) / Me.ClientSize.Height

        Dim oXhudu As Double = Math.PI / (180 / xangle)
        Dim oYhudu As Double = Math.PI / (180 / yangle)
        centX = (dblwindowLeft + dblwindowRight) / 2.0
        centY = (dblwindowTop + dblwindowBottom) / 2.0
        Dim btm As Double = 100.0 * Math.Cos(oXhudu)
        eye_y = 100 * Math.Sin(oXhudu) + ents_centerC.y
        eye_x = btm * Math.Sin(oYhudu) + ents_centerC.x
        eye_z = btm * Math.Cos(oYhudu) + ents_centerC.z

        Dim oXhuduTmp As Double = Math.PI / (180 / xangle_Yais)
        Dim oYhuduTmp As Double = Math.PI / (180 / yangle_Yais)
        btm = r_tmp * Math.Cos(oXhuduTmp)
        pnt_Yais.y = r_tmp * Math.Sin(oXhuduTmp) + ents_centerC.y
        pnt_Yais.x = btm * Math.Sin(oYhuduTmp) + ents_centerC.x
        pnt_Yais.z = btm * Math.Cos(oYhuduTmp) + ents_centerC.z

        oXhuduTmp = Math.PI / (180 / xangle_Xais)
        oYhuduTmp = Math.PI / (180 / yangle_Xais)
        btm = r_tmp * Math.Cos(oXhuduTmp)
        pnt_Xais.y = r_tmp * Math.Sin(oXhuduTmp) + ents_centerC.y
        pnt_Xais.x = btm * Math.Sin(oYhuduTmp) + ents_centerC.x
        pnt_Xais.z = btm * Math.Cos(oYhuduTmp) + ents_centerC.z

        Vec_Screen_Y.setXYZ(pnt_Xais.x - eye_x, pnt_Xais.y - eye_y, pnt_Xais.z - eye_z)
        Vec_Screen_Y.Normalize()
        Vec_Screen_X.setXYZ(pnt_Yais.x - eye_x, pnt_Yais.y - eye_y, pnt_Yais.z - eye_z)
        Vec_Screen_X.Normalize()

        'ents_centerC.x = 100
        'ents_centerC.y = 0
        'ents_centerC.z = 0

        '��]���S�ɓ_��\��
        glPointSize(5)
        glColor3f(0, 0, 1)
        glBegin(GL_POINTS)
        glVertex3d(0, 0, 0)
        'glVertex3d(ents_centerC.x, ents_centerC.y, ents_centerC.z)
        glEnd()


        '���_��ݒ肷��A��]����ꍇ�͊p�x�ɂ��A���_�̌���������ύX���܂��̂ŁA��������f�K�v
        If Math.Abs(xangle) < 90.0 Then
            view_UpLow = 1.0
            gluLookAt(eye_x, eye_y, eye_z, ents_centerC.x, ents_centerC.y, ents_centerC.z, 0, 1.0, 0)
        Else
            If Math.Abs(xangle) >= 270.0 Then
                If Math.Abs(xangle) >= 360.0 Then
                    xangle = 0.0
                End If
                view_UpLow = 1.0
                gluLookAt(eye_x, eye_y, eye_z, ents_centerC.x, ents_centerC.y, ents_centerC.z, 0, 1.0, 0)
            Else
                view_UpLow = -1.0
                gluLookAt(eye_x, eye_y, eye_z, ents_centerC.x, ents_centerC.y, ents_centerC.z, 0, -1.0, 0)
            End If
        End If
        Trace.WriteLine("eyes : " & xangle & " " & yangle & " ---- " & eye_x & " " & eye_y & " " & eye_z & "  ----  " & ents_centerC.x & " " & ents_centerC.y & " " & ents_centerC.z)

        'ents_centerC = geoPmoto.Copy

        If yangle >= 360.0 Then
            yangle = 0
        End If
        'glScaled(sys_ScaleInfo.scale, sys_ScaleInfo.scale, sys_ScaleInfo.scale)
        If Bln_setCoord Then
            'glMatrixMode(GL_MODELVIEW)
            'glMultMatrixd(set_DblCoord)
        End If
        'glPushMatrix()
        'glMatrixMode(GL_MODELVIEW)
        'glMultMatrixd(sys_CoordInfo.mat)

        Dim ii As Integer, jj As Integer
        Dim blnAri As Boolean = False
        '#If 0 Then '--del.start----------------------12.5.10
        '        '���C�̕\��
        '        Dim ii As Integer, jj As Integer
        '        Dim blnAri As Boolean = False
        '        If nRays > 0 Then
        '            If entset_ray.blnVisiable Then
        '                For ii = 0 To nRays - 1
        '                    If gDrawRays(ii).blnDraw Then
        '                        '�N���b�N����ꍇ�ŁA�N���b�N���郌�C���ǂ����A���f����
        '                        glLoadName(Ray_Name + ii)
        '                        If model_pick = Ray_Name + ii Then
        '                            YCM_DrawRay(gDrawRays(ii), True)
        '                        Else
        '                            YCM_DrawRay(gDrawRays(ii), False)
        '                        End If
        '                    End If
        '                Next
        '            End If
        '        End If
        '#End If '--del.end------------------------12.5.10
        SetLight(eye_x, eye_y, eye_z)

        '�X�P�[������\��
        If Not gScaleLine Is Nothing Then
            glLineWidth(LineWidth_Line * 5) '�����̕�
            YCM_DrawUserLine(gScaleLine, False)
        End If


        '���C���̕\��
        If nUserLines > 0 Then
            Try '20161110 baluu ADD start
                glLineWidth(LineWidth_Line) '�����̕�
                'If entset_line.blnVisiable Then
                For ii = 0 To nUserLines - 1
                    If gDrawUserLines(ii).binDelete = False Then
                        If gDrawUserLines(ii).blnDraw Then
                            If ((gDrawUserLines(ii).elmType = 0) And entset_line.blnVisiable) Or _
                                ((gDrawUserLines(ii).elmType = 1) And entset_line_CAD.blnVisiable) Then
                                '20170209 baluu add start
                                If (DrawUserLine = True And gDrawUserLines(ii).createType = 0) Or _
                                   (DrawSunpoLine = True And gDrawUserLines(ii).createType = 1) Or _
                                   (DrawSekkeiLine = True And gDrawUserLines(ii).createType = 2) Then
                            'elmType = 0�F����_�C�Ӑ}�`/elmType = 1:����_CAD�}�`�f20121126
                                    '�N���b�N����ꍇ�ŁA�N���b�N���郉�C�����ǂ����A���f����
                                    If (gDrawUserLines(ii).blnDelOnGlwin = False) Then
                                        glLoadName(Drawray_Name + ii)
                                        gDrawUserLines(ii).model_pick = Drawray_Name + ii
                                        If model_SingleSelct Then
                                            If model_pick = Drawray_Name + ii Then
                                                YCM_DrawUserLine(gDrawUserLines(ii), True)
                                            Else
                                                YCM_DrawUserLine(gDrawUserLines(ii), False)
                                            End If
                                        Else
                                            blnAri = False
                                            For jj = 0 To model_picks.Count - 1
                                                If model_picks.Item(jj) = Drawray_Name + ii Then
                                                    blnAri = True
                                                End If
                                            Next
                                            If blnAri Then
                                                YCM_DrawUserLine(gDrawUserLines(ii), True)
                                            Else
                                                YCM_DrawUserLine(gDrawUserLines(ii), False)
                                            End If

                                            If model_pick = Drawray_Name + ii Then
                                                ent_click.TransToLine(gDrawUserLines(ii))
                                                'Arrent_click.Add(ent_click)
                                                bln_EntClickPoint = True
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            
            Catch ex As Exception
            
 			End Try '20161110 baluu ADD end
        End If
        'End If

        'glEnable(GL_LIGHT0)
        'glEnable(GL_LIGHTING)

        '�J�����̕\��
        c_click_camers = New CCamera
        If nCamers > 0 Then
            If entset_camera.blnVisiable Then
                For ii = 0 To nCamers - 1
                    If gDrawCamers(ii).blnDraw Then
                        '�N���b�N����ꍇ�ŁA�N���b�N����J�������ǂ����A���f����
                        glLoadName(Camera_Name + ii)
                        '20170209 baluu add start
                        If camera_model_pick = Camera_Name + ii Then
                            YCM_DrawCamer(gDrawCamers(ii), True)
                        Else
                            YCM_DrawCamer(gDrawCamers(ii), False)
                        End If
                        If model_pick = Camera_Name + ii Then
                            c_click_camers = gDrawCamers(ii)
                        End If
                        '20170209 baluu add end
                        '20170209 baluu del start
                        'YCM_DrawCamer(gDrawCamers(ii))
                        'If model_pick = Camera_Name + ii Then
                        '    c_click_camers = gDrawCamers(ii)
                        'End If
                        '20170209 baluu del end
                        '20170215 baluu add start

                        Dim sc = (entset_camera.screensize / 100.0 * (dblwindowTop - dblwindowBottom)) + 0.001
                        Dim cameraLabel As New CLabelText(gDrawCamers(ii).x, gDrawCamers(ii).y + sc, gDrawCamers(ii).z)
                        cameraLabel.LabelName = "CAM:" & gDrawCamers(ii).ID

                        YCM_DrawLabelText(cameraLabel, False, True)
                        '20170215 baluu add end

                    End If
                Next
            End If
        End If
        If c_click_camers.blnSel Then bln_ClickCamera = True
        'glDisable(GL_LIGHT0)
        'glDisable(GL_LIGHTING)

        '#If 0 Then '--del.start----------------------12.5.10
        '        '�v���_�̕\��
        '        c_click = New CLookPoint
        '        'c_click.mid = -1
        '        If nLookPoints > 0 Then
        '            If entset_point.blnVisiable Then
        '                For ii = 0 To nLookPoints - 1
        '                    If gDrawPoints(ii).blnDraw Then
        '                        '�N���b�N����ꍇ�ŁA�N���b�N����v���_���ǂ����A���f����
        '                        glLoadName(Point_Name + ii)
        '                        If model_pick = Point_Name + ii Then
        '                            YCM_DrawLookPoint(gDrawPoints(ii), True)
        '                            c_click = gDrawPoints(ii)
        '                        Else
        '                            YCM_DrawLookPoint(gDrawPoints(ii), False)
        '                        End If
        '                    End If
        '                Next
        '            End If
        '        End If
        '        If c_click.mid <> -1 Then bln_ClickPoint = True

        '        '���x���̕\��
        '        If nLabelText > 0 Then
        '            If entset_label.blnVisiable Then
        '                For ii = 0 To nLabelText - 1
        '                    If gDrawLabelText(ii).blnDraw Then
        '                        '�N���b�N����ꍇ�ŁA�N���b�N���郉�x�����ǂ����A���f����
        '                        glLoadName(Label_Name + ii)
        '                        If model_pick = Label_Name + ii Then
        '                            YCM_DrawLabelText(gDrawLabelText(ii), True)
        '                            'c_click = gDrawPoints(ii)
        '                        Else
        '                            YCM_DrawLabelText(gDrawLabelText(ii), False)
        '                        End If
        '                    End If
        '                Next
        '            End If
        '        End If
        '#End If '--del.end------------------------12.5.10
#If 1 Then '--ins.rep.start---------------------12.5.10
        '�v���_�A���x���A���C�̕\��
        '20161114 baluu add start
        c_click = New CLookPoint
        '20161114 baluu add end
        Call drawBasePoint(c_click) '���x���̕\��
        If c_click.mid <> -1 Then bln_ClickPoint = True
        '���\���p�v���_�̕\��
        '20170217 baluu add start
        If DrawUserLine = True Then
            Call drawCurrUserPoints()
        End If
        '20170217 baluu add end
        'Call drawCurrUserPoints() '20170217 baluu del
        'SUSANO ADD START
        '�݌v�_�̕\��
        '20170217 baluu add start
        If DrawSekkeiLine = True Then
            drawSekkeiPoint(c_click)
        End If
        '20170217 baluu add end
        'drawSekkeiPoint(c_click) '20170217 baluu del
        'SUSANO ADD END
#End If '--ins.rep.start------------------------12.5.10
        '�~
        If entset_circle.blnVisiable Then
            glLineWidth(circleWidth_Line) '13.2.1�ǉ��@�~�̐���
            'If ncirclepoint > 0 Then
            For ii = 0 To ncirclepoint - 1
                If gDrawCirclepoint(ii).blnDraw Then
                    'If model_pick = Circle_Name + ii Then
                    '    YCM_DrawCirclepoint(gDrawCirclepoint(ii), dblxangle(ii), dblyangle(ii), True)
                    'Else
                    'If ((gDrawCirclepoint(ii).elmType = 0) And entset_circle.blnVisiable) Or _
                    '    ((gDrawCirclepoint(ii).elmType = 1) And entset_circle_CAD.blnVisiable) Then
                    '    'elmType = 0�F�~_�C�Ӑ}�`/elmType = 1:�~_CAD�}�`
                    YCM_DrawCirclepoint(gDrawCirclepoint(ii), dblxangle(ii), dblyangle(ii), False)
                    'End If
                    'End If
                End If
            Next
        End If
        If nCircle > 0 Then
            Try '20161110 baluu ADD start
                For ii = 0 To nCircle - 1
                    If gDrawCircle(ii).blnDraw Then
                        glLoadName(Circle_Name + ii)
                        If model_pick = Circle_Name + ii Then
                            YCM_DrawCircle(gDrawCircle(ii), True)
                        Else
                            YCM_DrawCircle(gDrawCircle(ii), False)
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try '20161110 baluu ADD end

        End If
        If nCircleNew > 0 Then
            Try '20161110 baluu ADD start

                For ii = 0 To nCircleNew - 1
                    If ((gDrawCircleNew(ii).elmType = 0) And entset_circle.blnVisiable) Or _
                            ((gDrawCircleNew(ii).elmType = 1) And entset_circle_CAD.blnVisiable) Then
                        'elmType = 0�F�~_�C�Ӑ}�`/elmType = 1:�~_CAD�}�`
                        If gDrawCircleNew(ii).binDelete = False Then
                            If gDrawCircleNew(ii).blnDraw Then
                                If gDrawCircleNew(ii).blnDelOnGlwin = False Then    'Add Kiryu 
 								'20170209 baluu add start
                                If (DrawUserLine = True And gDrawCircleNew(ii).createType = 0) Or _
                                   (DrawSunpoLine = True And gDrawCircleNew(ii).createType = 1) Or _
                                   (DrawSekkeiLine = True And gDrawCircleNew(ii).createType = 2) Then
                                    glLoadName(Circle_Name + ii)
                                    gDrawCircleNew(ii).model_pick = Circle_Name + ii 'Add Kiryu
                                    If model_SingleSelct Then
                                        If model_pick = Circle_Name + ii Then
                                            YCM_DrawCircleNew(gDrawCircleNew(ii), True)
                                        Else
                                            YCM_DrawCircleNew(gDrawCircleNew(ii), False)
                                        End If
                                    Else

                                        blnAri = False
                                        For jj = 0 To model_picks.Count - 1
                                            If model_picks.Item(jj) = Circle_Name + ii Then
                                                blnAri = True
                                            End If
                                        Next
                                        If blnAri Then
                                            YCM_DrawCircleNew(gDrawCircleNew(ii), True)
                                        Else
                                            YCM_DrawCircleNew(gDrawCircleNew(ii), False)
                                        End If
                                        If model_pick = Circle_Name + ii Then
                                            ent_click.TransToCIRCLE(gDrawCircleNew(ii))
                                            'Arrent_click.Add(ent_click)
                                            bln_EntClickPoint = True
                                        End If

                                    End If

                                End If
							End If
                            '20170209 baluu add end
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try '20161110 baluu ADD end

        End If
        'End If

        'glPopMatrix()

        'Dim sx As Integer = Me.ClientSize.Height
        'Dim posx As Integer, posy As Integer, posz As Integer
        'YCM_PickPoint2TruePoint(20, sx - 20, posx, posy, posz)
        'glPushMatrix()
        'glMatrixMode(GL_MODELVIEW)
        'glMultMatrixd(sys_CoordInfo.mat)

        '���W���̍쐬
        Dim dblcolor3fxyz(2) As Double
        Dim dblvertex3dxyz(2) As Double
        Dim dblpoint_OR(2) As Double
        Dim dblpoint_X(2) As Double
        Dim dblpoint_Y(2) As Double
        Dim dblpoint_Z(2) As Double
        Dim geoP As New GeoPoint, geoPX_Tmp, geoPY_Tmp, geoPZ_Tmp As New GeoPoint
        geoP.setXYZ(0, 0, 0)
        geoPX_Tmp.setXYZ(CSng(0 + 1.0 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(0), CSng(0))
        geoPY_Tmp.setXYZ(CSng(0), CSng(0 + 1 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)), CSng(0))
        geoPZ_Tmp.setXYZ(CSng(0), CSng(0), CSng(0 + 1 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)))
        Dim mat_XYZ As GeoMatrix, mat_lib As New MatLib
        Dim mat_dbl(0 To 15) As Double
        Dim LineXYZ As Double = 1.0F
        glLineWidth(LineXYZ) '13.2.1 ���W���̐��̕���ݒ�
        '���W���̉�]Matrix���쐬����
        mat_XYZ = mat_lib.SetByDoubleMat(sys_CoordInfo.mat)
        mat_XYZ.Invert()
        geoP = geoP.GetTransformed(mat_XYZ)
        geoPX_Tmp = geoPX_Tmp.GetTransformed(mat_XYZ)
        geoPY_Tmp = geoPY_Tmp.GetTransformed(mat_XYZ)
        geoPZ_Tmp = geoPZ_Tmp.GetTransformed(mat_XYZ)

        glBegin(GL_LINES)
        'glColor3f(1.0F, 0.0F, 0.0F) '13.2.1�C��
        Dim Mat_ColorRED() As Single = {1.0F, 0.0F, 0.0F, 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_ColorRED)
        glVertex3d(geoP.x, geoP.y, geoP.z)
        'glColor3f(1.0F, 0.0F, 0.0F) '13.2.1�C��
        glVertex3d(geoPX_Tmp.x, geoPX_Tmp.y, geoPX_Tmp.z)
        glEnd()

        glBegin(GL_LINES)
        'glColor3f(0.0F, 1.0F, 0.0F) '13.2.1�C��
        Dim Mat_ColorGREEN() As Single = {0.0F, 1.0F, 0.0F, 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_ColorGREEN)
        glVertex3d(geoP.x, geoP.y, geoP.z)
        'glColor3f(0.0F, 1.0F, 0.0F) '13.2.1�C��
        glVertex3d(geoPY_Tmp.x, geoPY_Tmp.y, geoPY_Tmp.z)
        glEnd()

        glBegin(GL_LINES)
        'glColor3f(0.0F, 0.0F, 1.0F) '13.2.1�C��
        Dim Mat_ColorBLUE() As Single = {0.0F, 0.0F, 1.0F, 1.0}
        glMaterialfv(GL_FRONT, GL_AMBIENT, Mat_ColorBLUE)
        glVertex3d(geoP.x, geoP.y, geoP.z)
        'glColor3f(0.0F, 0.0F, 1.0F) '13.2.1�C��
        glVertex3d(geoPZ_Tmp.x, geoPZ_Tmp.y, geoPZ_Tmp.z)
        glEnd()

        dblcolor3fxyz(0) = 1.0F
        dblcolor3fxyz(1) = 0.0F
        dblcolor3fxyz(2) = 0.0F
        'dblvertex3dxyz(0) = 0 + 1.0 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)
        'dblvertex3dxyz(1) = 0
        'dblvertex3dxyz(2) = 0
        dblvertex3dxyz(0) = geoPX_Tmp.x
        dblvertex3dxyz(1) = geoPX_Tmp.y
        dblvertex3dxyz(2) = geoPX_Tmp.z
        'X�������쐬����
        YCM_DrawLabelTextXYZ(dblcolor3fxyz, dblvertex3dxyz, "X")
        dblcolor3fxyz(0) = 0.0F
        dblcolor3fxyz(1) = 1.0F
        dblcolor3fxyz(2) = 0.0F
        'dblvertex3dxyz(0) = 0
        'dblvertex3dxyz(1) = 0 + 1.0 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)
        'dblvertex3dxyz(2) = 0
        dblvertex3dxyz(0) = geoPY_Tmp.x
        dblvertex3dxyz(1) = geoPY_Tmp.y
        dblvertex3dxyz(2) = geoPY_Tmp.z
        'Y�������쐬����
        YCM_DrawLabelTextXYZ(dblcolor3fxyz, dblvertex3dxyz, "Y")
        dblcolor3fxyz(0) = 0.0F
        dblcolor3fxyz(1) = 0.0F
        dblcolor3fxyz(2) = 1.0F
        'dblvertex3dxyz(0) = 0
        'dblvertex3dxyz(1) = 0
        'dblvertex3dxyz(2) = 0 + 1.0 * 10 / 100 * (View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)
        dblvertex3dxyz(0) = geoPZ_Tmp.x
        dblvertex3dxyz(1) = geoPZ_Tmp.y
        dblvertex3dxyz(2) = geoPZ_Tmp.z
        'Z�������쐬����
        YCM_DrawLabelTextXYZ(dblcolor3fxyz, dblvertex3dxyz, "Z")
        'glPopMatrix()
        ''Dim GeoPoin1 As New GeoPoint
        'GeoPoin1 = gppoint
        'YCM_Drawarea(GeoPoin1)

        '�͈͎w��̋@�\�����s�ł��邩�ǂ����A���f����
        If m_blnCommandArea = True And intgppointarea = 1 Then
            If Not (gppointend.x = 0 And gppointend.y = 0 And gppointend.z = 0) Then '20170223 baluu add
                Dim geopointleftup As New GeoPoint
                geopointleftup.x = gppointstart.x
                geopointleftup.y = gppointstart.y
                geopointleftup.z = gppointstart.z
                Dim geopoint1rightup As New GeoPoint
                geopoint1rightup.x = gppointend.x
                geopoint1rightup.y = gppointstart.y
                geopoint1rightup.z = gppointstart.z
                Dim geopointleftdown As New GeoPoint
                geopointleftdown.x = gppointstart.x
                geopointleftdown.y = gppointend.y
                geopointleftdown.z = gppointstart.z
                Dim geopoint1rightdown As New GeoPoint
                geopoint1rightdown.x = gppointend.x
                geopoint1rightdown.y = gppointend.y
                geopoint1rightdown.z = gppointstart.z

                YCM_Drawarea(geopointleftup, geopoint1rightup)
                YCM_Drawarea(geopointleftup, geopointleftdown)
                YCM_Drawarea(geopointleftdown, geopoint1rightdown)
                YCM_Drawarea(geopoint1rightup, geopoint1rightdown)
            End If '20170223 baluu add
        End If
        '20170110 baluu add start
        Try '20170224 baluu add
            If Not Model3dPath = Nothing Then
                If ReloadModel3dPath = True Then
                    modelPoints = Nothing
                End If
                YCM_DrawObjModel3d(Model3dPath, ReloadModel3dPath)
                ReloadModel3dPath = False
            End If
        Catch ex As Exception '20170224 baluu add
        End Try '20170224 baluu add
        '20170110 baluu add end
    End Sub

    '======================================================================    
    ' �@�\�F�w�肳�ꂽ�v���_�Ɋ֘A���郉�x���A���C��`�悷��
    ' �����F
    '   iPos    [I/ ] �v���_�̃C���f�b�N�X
    '======================================================================    
    Private Sub drawBasePoint(ByRef c_click As CLookPoint)
        '���v���_�̕\����
        ' Dim ii As Long, Mid As Long, ind As Long, iPosDraw As Integer
        Dim ii As Long, Mid As Long, iPosDraw As Integer
        Dim ���W�l�ꗗ�I����� As Boolean
        Dim bIsvisible As Boolean
        ''��20121128�@���x�N�g���A�}�e���A���A���C�g�̐ݒ�(YCM_3Dview.vb��SetLight,SetMaterial?)----------------------------------
        ''���̒�`
        ''���̒l��z�񂷂�
        'Dim glfLightAmbient As Single() = New Single(3) {0.1, 0.1, 0.1, 1.0}
        'Dim glfLightDiffuse As Single() = New Single(3) {0.7, 0.7, 0.7, 1.0}
        'Dim glfLightSpecular As Single() = New Single(3) {0.0, 0.0, 0.0, 1.0}
        'glLightfv(GL_LIGHT0, GL_AMBIENT, glfLightAmbient)
        'glLightfv(GL_LIGHT0, GL_DIFFUSE, glfLightDiffuse)
        'glLightfv(GL_LIGHT0, GL_SPECULAR, glfLightSpecular)
        'glEnable(GL_LIGHTING)
        'glEnable(GL_LIGHT0)
        ''�}�e���A���̒�`
        ''���̔��˂ɑ΂���l���i�[�����z����쐬����BRGBA�̒l�B	
        'Dim glfMaterialColor As Single() = New Single(3) {0.0, 0.0, 1.0, 1.0}
        ''glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT_AND_DIFFUSE, glfMaterialColor)
        'glMaterialfv(GL_FRONT, GL_SHININESS, 90.0F)
        'glEnable(GL_COLOR_MATERIAL)
        ''��20121128�@���x�N�g���A�}�e���A���A���C�g�̐ݒ�(YCM_3Dview.vb��SetLight,SetMaterial?)----------------------------------

        'c_click = New CLookPoint '20161114 baluu edit
        For ii = 0 To (nLookPoints - 1)   '�v���_��
            '[Type]�^�[�Q�b�g��� =1:�V���O���^�[�Q�b�g�A=2:�R�[�h�^�[�Q�b�g�A=9�F���[�U�ǉ��_
            bIsvisible = (gDrawPoints(ii).type = 1) And (entset_point.blnVisiable)
            bIsvisible = bIsvisible Or (gDrawPoints(ii).type = 2) And (CordTargetIsvisible)
            bIsvisible = bIsvisible Or (gDrawPoints(ii).type = 9) And (entset_pointUser.blnVisiable)
            bIsvisible = gDrawPoints(ii).blnDraw '20170302 baluu add
            If gDrawPoints(ii).createType = 1 And bIsvisible = True Then '20170221 baluu add start
                bIsvisible = DrawUserLine
            End If '20170221 baluu add end
            If gDrawPoints(ii).createType = 0 And bIsvisible = True Then '20170221 baluu add start
                bIsvisible = DrawSunpoLine
            End If '20170221 baluu add end
            'If (gDrawPoints(ii).flgLabel = 1) Then  '��\������\��
            If (bIsvisible = True) Then
                Mid = gDrawPoints(ii).mid
                ���W�l�ꗗ�I����� = True ' getDataVertexDrawFlag(Mid, ind)
                glLoadName(Point_Name + ii)
                'If(�I������Ă���v���_)Then
                '#If DEBUG Then  '�f�o�b�O����CT��\�����Ȃ�
                '                If gDrawPoints(ii).tid > 10000 Then
                '                    If gDrawPoints(ii).type <> 2 Then
                '#End If
                If model_pick <> 0 Then
                    model_pick = model_pick
                End If
                If model_pick = Point_Name + ii Then
                    '�E�v���_�̕\���i�I����ԁj
                    iPosDraw = YCM_DrawLookPoint(gDrawPoints(ii), True)
                    c_click = gDrawPoints(ii)
                    '�E���C�\��
                    Call displayRayByMID(Mid)
                    '�E���x���\��
                    If (iPosDraw > 0) Then Call displayLabelByMID(Mid)
                Else
                    If (���W�l�ꗗ�I����� = True) Then
                        '�E�v���_�̕\���i��I����ԁj
                        iPosDraw = YCM_DrawLookPoint(gDrawPoints(ii), False)
                        '�E�v���_�̃��x����\������
                        If (iPosDraw > 0) Then Call displayLabelByMID(Mid)
                    Else
                        '�E���W�l�ꗗ�őI������Ă��Ȃ��v���_�̃��x���A���C�͕\�����Ȃ�
                    End If
                End If
                '#If DEBUG Then
                '                    End If
                '#End If
            End If
        Next ii
        '���J�����I�����̏������J�����\���R�}���h���s���ɂ͈ȉ��̏����͍s��Ȃ�
        If (bln_ClickCamera) Then
            'c_click_camers.blnSel
            'If(blnThreadRun = False) Then 
            '�I������Ă���J�������烌�C��\������

        End If
    End Sub

    Private Sub drawCurrUserPoints()
        Try '20161115 baluu add
            Dim ii As Long
            Dim wLabelText As New CLabelText
            If (nCurrUserMovePoints > 0) Then
                For ii = 0 To (nCurrUserMovePoints - 1)   '���\���p�v���_��
                    '���v���_�̕\���i��I����ԁj
                    If Not gCurrUserMovePoints(ii) Is Nothing Then '20161115 baluu add
                        YCM_DrawLookPoint(gCurrUserMovePoints(ii), False)
                        wLabelText.blnDraw = True
                        wLabelText.LabelName = gCurrUserMovePoints(ii).LabelName
                        wLabelText.x = gCurrUserMovePoints(ii).x
                        wLabelText.y = gCurrUserMovePoints(ii).y
                        wLabelText.z = gCurrUserMovePoints(ii).z
                        YCM_DrawLabelText(wLabelText, False)
                    End If '20161115 baluu add
                Next ii
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try '20161115 baluu add
        
    End Sub
    ' MID������W�l�ꗗ�̑I����Ԃ𓾂�
    Private Function getDataVertexDrawFlag(ByVal MID As Long, ByRef ind As Long) As Boolean
        Dim numDV As Long, ii As Long
        getDataVertexDrawFlag = False
        ind = -1
        numDV = Data_Point.DGV_DV.Rows.Count
        For ii = 0 To numDV - 1
            If (Data_Point.DGV_DV.Rows(ii).Cells(5).Value = MID) Then
                ''getDataVertexDrawFlag = Data_Point.DGV_DV.Rows(ii).Cells(0).Value
                getDataVertexDrawFlag = Data_Point.DGV_DV.Rows(ii).Cells(0).EditedFormattedValue
                ind = ii
                Exit Function
            End If
        Next
    End Function

    'MID���w�肵�ă��x����\������
    Private Sub displayLabelByMID(ByVal MID As Long)
        Dim ii As Long
        If entset_label.blnVisiable Then
            If nLabelText > 0 Then
                For ii = 0 To nLabelText - 1
                    If (gDrawLabelText(ii).mid = MID) Then
                        If gDrawLabelText(ii).DispFlag = True Then
                            ''If (gDrawLabelText(ii).blnDraw = True) Then
                            YCM_DrawLabelText(gDrawLabelText(ii), False)
                            Exit Sub
                            ''End If
                            'glLoadName(Label_Name + ii)
                            'If model_pick = Label_Name + ii Then
                            '    YCM_DrawLabelText(gDrawLabelText(ii), True)
                            'Else
                            '    YCM_DrawLabelText(gDrawLabelText(ii), False)
                            'End If
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    'MID���w�肵�ă��C��\������
    Private Sub displayRayByMID(ByVal MID As Long)
        Dim ii As Long
        If (entset_ray.blnVisiable = True) Then
            If nRays > 0 Then
                For ii = 0 To nRays - 1
                    If (gDrawRays(ii).PointID = MID) Then
                        'If (gDrawRays(ii).blnDraw = True) Then
                        YCM_DrawRay(gDrawRays(ii), False)
                        'End If
                    End If
                Next
            End If
        End If
    End Sub

    '�J�������w�肵�ă��C��\������
    ' gDrawCamers(ii).ID 

    '============================================================================

    '�}�e���A���̐ݒ�
    Private Sub SetMaterial()

        '�g�U��(Diffuse)
        Dim matdiff() As Single = {1, 1, 1, 1}
        glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, matdiff)

        '���ʌ�(Specular)
        Dim matspec() As Single = {1, 1, 1, 1}
        glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, matspec)

        '���ʌW��
        Dim matshin() As Single = {25}
        glMaterialfv(GL_FRONT_AND_BACK, GL_SHININESS, matshin)

        '����(Ambient)
        Dim matambi() As Single = {0.4, 0.4, 0.4, 1}
        glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, matambi)

    End Sub
    '�`��v����
    'Private Sub YCM_3DView_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
    'End Sub

    '�N���b�N�̃C�x���g
    Private Sub YCM_3DView_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        Dim hits As Integer, viewport(4) As Integer
        Dim selectBuff(64) As UInteger
        Dim sx As Integer = Me.ClientSize.Height
        Me.Select()
        Me.Focus()
        If m_blnCommandArea = False Then
            If crl_mode = ControlMode.Normal And e.Button = Windows.Forms.MouseButtons.Left Then
                '���̂�I������@�\�ŁA
                blnSelectMode = True
                glSelectBuffer(64, selectBuff)
                glGetIntegerv(GL_VIEWPORT, viewport)
                'glDisable(GL_LIGHTING)
                glMatrixMode(GL_PROJECTION)
                glPushMatrix()
                glRenderMode(GL_SELECT)
                glLoadIdentity()
                '�I��Matrix���쐬����
                gluPickMatrix(e.X, sx - e.Y, 10, 10, viewport)
                glOrtho(dblwindowLeft, dblwindowRight, dblwindowBottom, dblwindowTop, 0.0001, 100000)

                glMatrixMode(GL_MODELVIEW)
                Draw()
                '�I�����̖̂��O���擾����
                hits = glRenderMode(GL_RENDER)
                If hits > 0 Then
                    Dim blnPoint_ScreenSelect As Boolean = False
                    For ii As Integer = 0 To hits - 1
                        model_pick = selectBuff(3 + 4 * ii)
                        If model_pick >= 1 And model_pick < 10000 Then
                            blnPoint_ScreenSelect = True
                            Exit For
                        End If
                    Next
                    If blnPoint_ScreenSelect Then
                        For ii As Integer = 0 To hits - 1
                            model_pick = selectBuff(3 + 4 * ii)
                            If model_pick >= 1 And model_pick < 10000 Then '20170216 baluu edit start
                                MainFrm.AllDraw() '20170215 baluu add 
                                Exit For
                            End If '20170216 baluu edit end
                        Next
                    Else
                        model_pick = selectBuff(3)
                    End If
                    If model_SingleSelct = False Then
                        model_picks.Add(model_pick)
                    End If
                End If
                glMatrixMode(GL_PROJECTION)
                glPopMatrix()
                glMatrixMode(GL_MODELVIEW)
                blnSelectMode = False
                Draw()
                'glEnable(GL_LIGHTING)
                'Dim geopoint2 As New GeoPoint
                'Dim gppoint As New GeoPoint
                'geopoint2.x = e.X
                'geopoint2.y = e.Y
                'YCM_PickPoint2TruePoint(e.X, e.Y, gppoint.x, gppoint.y, gppoint.z)
                ''IOUtil.GetPoint1(geopoint2, geopoint1)
                'MsgBox(gppoint.x & "," & gppoint.y & "," & gppoint.z)

                If IOUtil.blnSelectMode Then
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        IOUtil.blnPickPoint = True
                        IOUtil.pick_P = c_click
                        model_pick = 0
                    End If
                End If
                If IOUtil.blnSelectMode Then
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        IOUtil.binPickCamer = True
                        IOUtil.pick_C = c_click_camers
                    End If

                End If
                If IOUtil.blnSelectMode_fram Then
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        IOUtil.binpickfram = True
                        IOUtil.pick_F.x = e.X
                        IOUtil.pick_F.y = e.Y
                        IOUtil.pick_F.z = 0
                    End If
                End If
            End If
        End If
        '20170223 baluu del start
        '  **************************************area**************************************
        '�͈͎w������邩�ǂ����A
        'If m_blnCommandArea = True And intgppointarea = 0 Then
        '    'YCM_PickPoint2TruePoint(e.X, e.Y - 50, gppointstart.x, gppointstart.y, gppointstart.z) '20170222 baluu del
        '    YCM_PickPoint2TruePoint(e.X, e.Y, gppointstart.x, gppointstart.y, gppointstart.z) '20170222 baluu add
        '    intgppointarea = 1
        '    cp_fram_start.x = e.X
        '    cp_fram_start.y = e.Y
        '    Exit Sub
        'End If
        'If intgppointarea = 1 And m_blnCommandArea = True Then
        '    intgppointarea = 0
        '    cp_fins_start = gppointstart
        '    cp_fins_end = gppointend
        '    Dim fram_old_x As Double = Me.ClientSize.Width
        '    Dim fram_old_y As Double = Me.ClientSize.Height - 50
        '    Dim fram_new_x As Double = 0.0
        '    Dim fram_new_y As Double = 0.0

        '    fram_new_x = Math.Abs(cp_fram_end.x - cp_fram_start.x)
        '    fram_new_y = Math.Abs(cp_fram_end.y - cp_fram_start.y)

        '    Dim dblwidth_new_old As Double = fram_new_x / fram_old_x
        '    Dim dblhetght_new_old As Double = fram_new_y / fram_old_y
        '    Dim dbltemp_new_old As Double
        '    If dblwidth_new_old > dblhetght_new_old Then
        '        dbltemp_new_old = dblhetght_new_old
        '    Else
        '        dbltemp_new_old = dblwidth_new_old
        '    End If

        '    '20170222 baluu add start
        '    If m_blnSelectDelArea = False Then
        '        Dim geoMidScreen As New GeoPoint
        '        Call geoMidScreen.setXYZ(cp_fins_start.x / 2 + cp_fins_end.x / 2, (cp_fins_start.y + cp_fins_end.y) / 2, (cp_fins_start.z + cp_fins_end.z) / 2)
        '        'YCM_LookPointMatGeoPoint(cp_fins_start, resultMMNew)
        '        YCM_LookPointMatGeoPoint(geoMidScreen, resultMMNew)
        '        ents_centerC.x = geoMidScreen.x
        '        ents_centerC.y = geoMidScreen.y
        '        ents_centerC.z = geoMidScreen.z
        '        Dim dblwh As Double = Math.Abs((View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)) / Math.Abs((View_Dilaog.dblwindowRight - View_Dilaog.dblwindowLeft))

        '        If Math.Abs((cp_fins_start.x - cp_fins_end.x)) * dblwh > Math.Abs((cp_fins_start.y - cp_fins_end.y)) Then
        '            View_Dilaog.dblwindowLeft = -Math.Abs((cp_fins_start.x - cp_fins_end.x)) * 0.5
        '            View_Dilaog.dblwindowRight = Math.Abs((cp_fins_start.x - cp_fins_end.x)) * 0.5
        '            View_Dilaog.dblwindowBottom = -Math.Abs((cp_fins_start.x - cp_fins_end.x)) * dblwh * 0.5
        '            View_Dilaog.dblwindowTop = Math.Abs((cp_fins_start.x - cp_fins_end.x)) * dblwh * 0.5
        '        Else
        '            View_Dilaog.dblwindowLeft = -Math.Abs((cp_fins_start.y - cp_fins_end.y)) / dblwh * 0.5
        '            View_Dilaog.dblwindowRight = Math.Abs((cp_fins_start.y - cp_fins_end.y)) / dblwh * 0.5
        '            View_Dilaog.dblwindowBottom = -Math.Abs((cp_fins_start.y - cp_fins_end.y)) * 0.5
        '            View_Dilaog.dblwindowTop = Math.Abs((cp_fins_start.y - cp_fins_end.y)) * 0.5
        '        End If
        '    Else
        '        DelPointsFrom3D(hits, viewport, selectBuff, sx, cp_fram_start, cp_fram_end)
        '    End If
        '    '20170222 baluu add end
        '    m_blnCommandArea = False
        '    m_blnSelectDelArea = False
        '    gppointend = New GeoPoint
        '    gppointstart = New GeoPoint
        '    Exit Sub
        'End If
        '**************************************area**************************************
        '20170223 baluu del end
        '**************************************circle**************************************
        If bincirclestart = True Then
            Select Case intcircle
                Case 0
                    YCM_PickPoint2TruePoint(e.X, e.Y - 50, gpcircle1.x, gpcircle1.y, gpcircle1.z)
                    intcircle = 1
                    ReDim Preserve gDrawCirclepoint(ncirclepoint)
                    gDrawCirclepoint(ncirclepoint) = New CcirclePoint
                    gDrawCirclepoint(ncirclepoint).x = gpcircle1.x
                    gDrawCirclepoint(ncirclepoint).y = gpcircle1.y
                    gDrawCirclepoint(ncirclepoint).z = gpcircle1.z
                    gDrawCirclepoint(ncirclepoint).mid = ncirclepoint
                    gDrawCirclepoint(ncirclepoint).blnDraw = True
                    ReDim Preserve dblxangle(ncirclepoint)
                    ReDim Preserve dblyangle(ncirclepoint)
                    dblxangle(ncirclepoint) = xangle
                    dblyangle(ncirclepoint) = yangle
                    ncirclepoint = ncirclepoint + 1


                Case 1
                    YCM_PickPoint2TruePoint(e.X, e.Y - 50, gpcircle2.x, gpcircle2.y, gpcircle2.z)
                    intcircle = 2
                    ReDim Preserve gDrawCirclepoint(ncirclepoint)
                    gDrawCirclepoint(ncirclepoint) = New CcirclePoint
                    gDrawCirclepoint(ncirclepoint).x = gpcircle2.x
                    gDrawCirclepoint(ncirclepoint).y = gpcircle2.y
                    gDrawCirclepoint(ncirclepoint).z = gpcircle2.z
                    gDrawCirclepoint(ncirclepoint).mid = ncirclepoint
                    gDrawCirclepoint(ncirclepoint).blnDraw = True
                    ReDim Preserve dblxangle(ncirclepoint)
                    ReDim Preserve dblyangle(ncirclepoint)
                    dblxangle(ncirclepoint) = xangle
                    dblyangle(ncirclepoint) = yangle
                    ncirclepoint = ncirclepoint + 1

                Case 2
                    YCM_PickPoint2TruePoint(e.X, e.Y - 50, gpcircle3.x, gpcircle3.y, gpcircle3.z)
                    intcircle = 3
                    ReDim Preserve gDrawCirclepoint(ncirclepoint)
                    gDrawCirclepoint(ncirclepoint) = New CcirclePoint
                    gDrawCirclepoint(ncirclepoint).x = gpcircle3.x
                    gDrawCirclepoint(ncirclepoint).y = gpcircle3.y
                    gDrawCirclepoint(ncirclepoint).z = gpcircle3.z
                    gDrawCirclepoint(ncirclepoint).mid = ncirclepoint
                    gDrawCirclepoint(ncirclepoint).blnDraw = True
                    ReDim Preserve dblxangle(ncirclepoint)
                    ReDim Preserve dblyangle(ncirclepoint)
                    dblxangle(ncirclepoint) = xangle
                    dblyangle(ncirclepoint) = yangle
                    Dim ORG As New GeoPoint
                    Dim ve As New GeoVector
                    Dim r As Double
                    ReDim Preserve gDrawCircle(nCircle)
                    CcDrawCircle(gpcircle1, gpcircle2, gpcircle3, ORG, r, ve)
                    gDrawCircle(nCircle) = New Ccircle
                    gDrawCircle(nCircle).org = ORG
                    gDrawCircle(nCircle).r = r
                    gDrawCircle(nCircle).x_angle = yangle
                    gDrawCircle(nCircle).y_angle = -xangle
                    gDrawCircle(nCircle).mid = nCircle
                    gDrawCircle(nCircle).blnDraw = True

                    bincirclestart = False
                    ncirclepoint = ncirclepoint + 1
                    nCircle = nCircle + 1
            End Select
        End If

        '**************************************circle**************************************

    End Sub


    '20170222 baluu add start
    Private Sub DelPointsFrom3D(hits As Integer, viewport() As Integer, selectBuff() As UInteger, sx As Integer, _
                                cp_start As GeoPoint, cp_end As GeoPoint)
        blnSelectMode = True
        Dim err = glGetError()
        glSelectBuffer(64, selectBuff)
        glGetIntegerv(GL_VIEWPORT, viewport)
        'glDisable(GL_LIGHTING)
        glMatrixMode(GL_PROJECTION)
        glPushMatrix()
        glRenderMode(GL_SELECT)
        glLoadIdentity()
        '�I��Matrix���쐬����
        Dim startX As Double = cp_start.x
        Dim startY As Double = cp_start.y
        If startX > cp_end.x Then
            startX = cp_end.x
        End If
        If startY > cp_end.y Then
            startY = cp_end.y
        End If
        startX = startX + (Math.Abs(cp_start.x - cp_end.x) / 2)
        startY = startY + (Math.Abs(cp_start.y - cp_end.y) / 2)
        gluPickMatrix(startX, sx - startY, Math.Abs(cp_start.x - cp_end.x), Math.Abs(cp_start.y - cp_end.y), viewport)
        glOrtho(dblwindowLeft, dblwindowRight, dblwindowBottom, dblwindowTop, 0.0001, 100000)
        glMatrixMode(GL_MODELVIEW)

        Draw()
        '�I�����̖̂��O���擾����
        hits = glRenderMode(GL_RENDER)
        If hits > 0 Then
            Dim indices As New List(Of Integer)
            Dim blnPoint_ScreenSelect As Boolean = False
            For ii As Integer = 0 To hits - 1
                Dim picked_id As Integer = selectBuff(3 + 4 * ii)
                If picked_id >= 1 And picked_id < 10000 Then
                    indices.Add(picked_id - 1)
                End If
            Next
            If indices.Count > 0 Then
                MainFrm.DeletePointsFrom3D(indices)
            End If
        End If
        glMatrixMode(GL_PROJECTION)
        glPopMatrix()
        glMatrixMode(GL_MODELVIEW)
        blnSelectMode = False
        Draw()
    End Sub
    '20170222 baluu add end

    Private Sub YCM_3DView_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ' MainProc()
    End Sub

    Private Sub YCM_3DView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove

        If bln_mouseClick Then

            'dblwindowLeft = dblwindowLeft - (e.X - click_P.x) / Me.Size.Width * Wheel_Speedx
            'dblwindowRight = dblwindowRight - (e.X - click_P.x) / Me.Size.Width * Wheel_Speedx
            'dblwindowTop = dblwindowTop + (e.Y - click_P.y) / Me.Size.Height * Wheel_Speedy
            'dblwindowBottom = dblwindowBottom + (e.Y - click_P.y) / Me.Size.Height * Wheel_Speedy
            If view_UpLow < 0 Then
                ents_centerC.Move(Vec_Screen_X.GetMultipled((e.X - click_P.x) * screen_scaleX))
            Else
                ents_centerC.Move(Vec_Screen_X.GetMultipled((click_P.x - e.X) * screen_scaleX))
            End If

            ents_centerC.Move(Vec_Screen_Y.GetMultipled((e.Y - click_P.y) * screen_scaleY))
            click_P.x = e.X
            click_P.y = e.Y

        End If

        If blnRotateClick Then
            xangle = xangle + (e.Y - click_P.y) / 20 * Wheel_Speedx
            yangle = yangle - (e.X - click_P.x) / 20 * Wheel_Speedy
            click_P.x = e.X
            click_P.y = e.Y
        End If

        If blnMoveClick Then
            'ent_x = ent_x - (e.X - click_P.x) / 590
            'ent_y = ent_y + (e.Y - click_P.y) / 590

            'dblwindowLeft = dblwindowLeft - (e.X - click_P.x) / Me.Size.Width * Wheel_Speedx
            'dblwindowRight = dblwindowRight - (e.X - click_P.x) / Me.Size.Width * Wheel_Speedx
            'dblwindowTop = dblwindowTop + (e.Y - click_P.y) / Me.Size.Height * Wheel_Speedy
            'dblwindowBottom = dblwindowBottom + (e.Y - click_P.y) / Me.Size.Height * Wheel_Speedy

            If view_UpLow < 0 Then
                ents_centerC.Move(Vec_Screen_X.GetMultipled((e.X - click_P.x) * screen_scaleX))
            Else
                ents_centerC.Move(Vec_Screen_X.GetMultipled((click_P.x - e.X) * screen_scaleX))
            End If
            ents_centerC.Move(Vec_Screen_Y.GetMultipled((e.Y - click_P.y) * screen_scaleY))
            click_P.x = e.X
            click_P.y = e.Y

        End If
        If m_blnCommandArea = True And intgppointarea = 1 Then
            'YCM_PickPoint2TruePoint(e.X, e.Y - 50, gppointend.x, gppointend.y, gppointend.z) ' 20170222 baluu del
            YCM_PickPoint2TruePoint(e.X, e.Y, gppointend.x, gppointend.y, gppointend.z) ' 20170222 baluu add
            'YCM_PickPoint2TruePoint2(e.X, e.Y, gppointend2.x, gppointend2.y, gppointend2.z)
            cp_fram_end.x = e.X
            cp_fram_end.y = e.Y
        End If
    End Sub

    Private Sub YCM_3DView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            bln_mouseClick = True
            click_P.SetXY(e.X, e.Y)
        End If

        If e.Button = Windows.Forms.MouseButtons.Left Then


            If crl_mode = ControlMode.Rotate Then
                click_P.SetXY(e.X, e.Y)
                blnRotateClick = True
            ElseIf crl_mode = ControlMode.Move Then
                click_P.SetXY(e.X, e.Y)
                blnMoveClick = True
            End If
            '20170223 baluu add start
            '**************************************area**************************************
            If m_blnCommandArea = True And intgppointarea = 0 Then
                YCM_PickPoint2TruePoint(e.X, e.Y, gppointstart.x, gppointstart.y, gppointstart.z)
                intgppointarea = 1
                cp_fram_start.x = e.X
                cp_fram_start.y = e.Y
                Exit Sub
            End If
            '**************************************area**************************************
            '20170223 baluu add end
        End If


        If e.Button = Windows.Forms.MouseButtons.Right Then
            click_P.SetXY(e.X, e.Y)
            blnRotateClick = True
            Me.Select()
            ' crl_mode = ControlMode.Normal
        End If


        '20160921 Add Kiryu �E�N���b�N���j���[�\���e�X�g
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Right
                '�R���e�L�X�g���j���[��\��������W
                befor_m = System.Windows.Forms.Cursor.Position

        End Select
        'Add End Kiryu 


    End Sub

    Private Sub YCM_3DView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            bln_mouseClick = False
        End If

        If e.Button = Windows.Forms.MouseButtons.Left Then


            If crl_mode = ControlMode.Rotate Then
                blnRotateClick = False
            ElseIf crl_mode = ControlMode.Move Then
                'click_P = New CPoint(e.X, e.Y)
                blnMoveClick = False
            End If
            '20170223 baluu add start
            '**************************************area**************************************
            If intgppointarea = 1 And m_blnCommandArea = True Then
                intgppointarea = 0
                cp_fins_start = gppointstart
                cp_fins_end = gppointend
                Dim fram_old_x As Double = Me.ClientSize.Width
                Dim fram_old_y As Double = Me.ClientSize.Height - 50
                Dim fram_new_x As Double = 0.0
                Dim fram_new_y As Double = 0.0

                fram_new_x = Math.Abs(cp_fram_end.x - cp_fram_start.x)
                fram_new_y = Math.Abs(cp_fram_end.y - cp_fram_start.y)

                Dim dblwidth_new_old As Double = fram_new_x / fram_old_x
                Dim dblhetght_new_old As Double = fram_new_y / fram_old_y
                Dim dbltemp_new_old As Double
                If dblwidth_new_old > dblhetght_new_old Then
                    dbltemp_new_old = dblhetght_new_old
                Else
                    dbltemp_new_old = dblwidth_new_old
                End If

                '20170222 baluu add start
                If m_blnSelectDelArea = False Then
                    Dim geoMidScreen As New GeoPoint
                    Call geoMidScreen.setXYZ(cp_fins_start.x / 2 + cp_fins_end.x / 2, (cp_fins_start.y + cp_fins_end.y) / 2, (cp_fins_start.z + cp_fins_end.z) / 2)
                    'YCM_LookPointMatGeoPoint(cp_fins_start, resultMMNew)
                    YCM_LookPointMatGeoPoint(geoMidScreen, resultMMNew)
                    ents_centerC.x = geoMidScreen.x
                    ents_centerC.y = geoMidScreen.y
                    ents_centerC.z = geoMidScreen.z
                    Dim dblwh As Double = Math.Abs((View_Dilaog.dblwindowTop - View_Dilaog.dblwindowBottom)) / Math.Abs((View_Dilaog.dblwindowRight - View_Dilaog.dblwindowLeft))

                    If Math.Abs((cp_fins_start.x - cp_fins_end.x)) * dblwh > Math.Abs((cp_fins_start.y - cp_fins_end.y)) Then
                        View_Dilaog.dblwindowLeft = -Math.Abs((cp_fins_start.x - cp_fins_end.x)) * 0.5
                        View_Dilaog.dblwindowRight = Math.Abs((cp_fins_start.x - cp_fins_end.x)) * 0.5
                        View_Dilaog.dblwindowBottom = -Math.Abs((cp_fins_start.x - cp_fins_end.x)) * dblwh * 0.5
                        View_Dilaog.dblwindowTop = Math.Abs((cp_fins_start.x - cp_fins_end.x)) * dblwh * 0.5
                    Else
                        View_Dilaog.dblwindowLeft = -Math.Abs((cp_fins_start.y - cp_fins_end.y)) / dblwh * 0.5
                        View_Dilaog.dblwindowRight = Math.Abs((cp_fins_start.y - cp_fins_end.y)) / dblwh * 0.5
                        View_Dilaog.dblwindowBottom = -Math.Abs((cp_fins_start.y - cp_fins_end.y)) * 0.5
                        View_Dilaog.dblwindowTop = Math.Abs((cp_fins_start.y - cp_fins_end.y)) * 0.5
                    End If
                Else
                    Dim hits As Integer, viewport(4) As Integer
                    Dim selectBuff(64) As UInteger
                    Dim sx As Integer = Me.ClientSize.Height
                    DelPointsFrom3D(hits, viewport, selectBuff, sx, cp_fram_start, cp_fram_end)
                End If
                '20170222 baluu add end
                m_blnCommandArea = False
                m_blnSelectDelArea = False
                m_blnDrawRegion = False '20170224 baluu add
                gppointend = New GeoPoint
                gppointstart = New GeoPoint
                Exit Sub
            End If
            '**************************************area**************************************
            '20170223 baluu add end
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If crl_mode <> ControlMode.Rotate Then
                crl_mode = ControlMode.Normal
                blnRotateClick = False
            End If
        End If

        '20160921 Add Kiryu �E�N���b�N���j���[�\���e�X�g
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Right

                '�R���e�L�X�g���j���[��\��������W
                Dim p As System.Drawing.Point = System.Windows.Forms.Cursor.Position
                If befor_m = p Then
                    '�w�肵����ʏ�̍��W�ʒu�ɃR���e�L�X�g���j���[��\������
                    Me.ContextMenuStrip1.Show(p)
                End If

        End Select
        '20160921 End Kiryu �E�N���b�N���j���[�\���e�X�g


        'If binareastart = True And intgppointarea = 2 Then
        '    intgppointarea = 0
        '    binareastart = False
        'End If
        'If binareastart = True Then
        '    binareastart = False
        'End If

    End Sub
    '�ˉe�ϊ��s��ƃr���[�|�[�g�̐ݒ�
    Private Sub SetProjectionAndViewport()

        '�X�N���[���T�C�Y�擾
        '���ݎ����擾
        Dim sx As Integer = Me.ClientSize.Width
        Dim sy As Integer = Me.ClientSize.Height ' - 50
        If (sx = 0) Or (sy = 0) Then Exit Sub '�\���s�\

        '�ˉe�ϊ��s��̐ݒ�
        Dim asp As Double = CDbl(sx) / CDbl(sy)     '�A�X�y�N�g��v�Z
        glMatrixMode(GL_PROJECTION)                 '�s�񃂁[�h���ˉe�ϊ��ɂ���
        glLoadIdentity()                            '���݂̍s��ɒP�ʍs��ɂ���
        glOrtho(dblwindowLeft, dblwindowRight, dblwindowBottom, dblwindowTop, 0.0001, 100000)
        'gluPerspective(45, asp, 0.5, 100000)      '���݂̍s��Ƀp�[�X�y�N�e�B�u�ϊ��s���������
        'glTranslatef(-EYEX, -EYEY, -EYEZ)           '���݂̍s��ɕ��s�ړ��s���������

        'glOrtho(-0.5, 0.5, -0.5, 0.5, 0.5, 100000)
        '�ȏ�ŁA���݂̍s��i���ˉe�ϊ��s��j�ɂ͉��L�̏�Ԃ��ݒ肳��܂��B
        '�E���W(EYEX,EYEY,EYEZ)����Z�������̕���������
        '�E����p�F45�x
        '�E�ŋߓ_�F1.0�i�������O�͕`�悳��Ȃ��j
        '�E�ŉ��_�F1000.0�i�����艜�͕`�悳��Ȃ��j
        glMatrixMode(GL_MODELVIEW)                  '�s�񃂁[�h�����f���r���[�ɖ߂�

        '�r���[�|�[�g�ݒ�
        '���̏����ɂ��A�E�B���h�E�T�C�Y���ύX����Ă��S�̂ɕ`�悳���悤�ɂȂ�܂��B
        glViewport(0, 0, sx, sy)
    End Sub

    Private Sub YCM_3DView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

        Dim sx As Double = Me.ClientSize.Width
        Dim sy As Double = Me.ClientSize.Height
        Dim pidx As Double, pidy As Double
        Dim lenx As Double, leny As Double
        Dim dix1 As Double, diy1 As Double
        Dim dix2 As Double, diy2 As Double
        Dim dix As Double, diy As Double
        Dim tt As Double = 1.3
        Dim dd As Double = 0.7
        pidx = (dblwindowLeft + dblwindowRight) / 2.0
        pidy = (dblwindowTop + dblwindowBottom) / 2.0
        lenx = dblwindowRight - dblwindowLeft
        leny = dblwindowTop - dblwindowBottom
        dix1 = lenx / sx : diy1 = leny / sy

        If e.Y > 0 And e.X > 0 Then '(1)�����}�E�X�̈ʒu��3D�r���[�ɂ��鎞�E�E�E�E
            If e.Delta > 0 Then
                lenx = lenx * dd : leny = leny * dd
                'If iii = 10 Then
                '    pointfar = pointfar * 0.1
                'End If
                'pointnear = pointnear * 0.1
                'pointfar = pointfar * 0.99
                'eye_z = eye_z * 0.9
                'scale = scale * 1.1
            Else

                lenx = lenx / dd : leny = leny / dd
                Wheel_Speedx = Wheel_Speedx / dd
                Wheel_Speedy = Wheel_Speedy / dd
                '
                'pointfar = pointfar * 0.9
                'eye_z = eye_z * 1.1
                'scale = scale * 0.9
            End If

            dix2 = lenx / sx : diy2 = leny / sy
            dix = (dix1 - dix2) * (e.X - sx / 2)
            diy = (diy1 - diy2) * (sy - e.Y - sy / 2)
            dblwindowLeft = pidx - lenx / 2.0 + dix
            dblwindowRight = pidx + lenx / 2.0 + dix
            dblwindowBottom = pidy - leny / 2.0 + diy
            dblwindowTop = pidy + leny / 2.0 + diy

            Wheel_Speedx = lenx
            Wheel_Speedy = leny
        Else '(2)�����}�E�X�̈ʒu��3D�r���[�ɂȂ����E�E�E�E
        End If

    End Sub

    Private Sub YCM_3DView_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        'Me.Select()
        Me.Focus()
    End Sub

    Private Sub YCM_3DView_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim sx As Integer = Me.ClientSize.Width
        Dim sy As Integer = Me.ClientSize.Height '- 50
        'glViewport(0, 0, sx, sy)
        glMatrixMode(GL_PROJECTION)
        glLoadIdentity()
        Dim sd As Double
        Dim lenx As Double, leny As Double
        leny = dblwindowTop - dblwindowBottom
        lenx = dblwindowRight - dblwindowLeft
        sd = leny * sx / sy

        dblwindowRight = dblwindowRight + sd - lenx

        Wheel_Speedx = sd
        Wheel_Speedy = leny

        glViewport(0, 0, sx, sy)
    End Sub

    Private Sub YCM_3DView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'If e.KeyCode <> Keys.Enter And e.KeyCode <> Keys.Escape And e.KeyCode <> Keys.ControlKey Then
        '    MainFrm.TBox_Data.Select()
        '    MainFrm.TBox_Data.Text = MainFrm.TBox_Data.Text + Strings.LCase(Strings.Chr(e.KeyValue))
        '    MainFrm.TBox_Data.SelectionStart = Len(MainFrm.TBox_Data.Text)
        'ElseIf e.KeyCode = Keys.ControlKey Then
        '    crl_mode = ControlMode.Rotate
        '    Me.Select()
        '    'blnCtrlKey = True
        'End If
        'If e.KeyCode = Keys.Enter Then
        '    If model_SingleSelct = False Then
        '        model_picks_over = True
        '    End If
        'End If

        'Add Kiryu �V���[�g�J�b�g�L�[��` CATS/1607H-a3-20160920
        Select Case e.KeyCode

            Case Keys.C  '[ C ]�J�����\��/��\���@
                Call MainFrm.ChkBoxDispCameraOnOff(CameraDispFlag)
                If CameraDispFlag = False Then
                    CameraDispFlag = True
                Else
                    CameraDispFlag = False
                End If

            Case Keys.N '[ N ]�݌v���W �\��/��\���@
                View_Dilaog.ToggleSekkeiLine()

            Case Keys.D0 '[ 0 ]���x���T�C�Y�g��@
                Call MainFrm.BtnDispLabelBig()

            Case Keys.D9 '[ 9 ]���x���T�C�Y�k���@
                Call MainFrm.BtnDispLabelSmall()

            Case Keys.D1 '[ 1 ]�J�����A�C�R���T�C�Y�g��
                Call MainFrm.BtnDispCameraBig()

            Case Keys.D2 '[ 2 ]�J�����A�C�R���T�C�Y�k���@
                Call MainFrm.BtnDispCameraSmall()

            Case Keys.OemPeriod '[ . ]�J�����A�C�R���T�C�Y�g��
                Call MainFrm.BtnDispMarkerBig()

            Case Keys.Oemcomma '[ , ]�J�����A�C�R���T�C�Y�k��
                Call MainFrm.BtnDispMarkerSmall()

            Case Keys.Space
                Call MainFrm.BtnDispZoomAll()

            Case Keys.Delete
                Call DeleteLineOnGlView()
                Call DeleteCircleOnGlView()

        End Select


        '        If e.KeyCode = Keys.Delete Then
        '            For ii As Integer = Circle_Name To Circle_Name + nCircle
        '                If model_pick = ii Then
        '                    gDrawCircle(ii - Circle_Name).blnDraw = False
        '                    gDrawCirclepoint((ii - Circle_Name) * 3).blnDraw = False
        '                    gDrawCirclepoint((ii - Circle_Name) * 3 + 1).blnDraw = False
        '                    gDrawCirclepoint((ii - Circle_Name) * 3 + 2).blnDraw = False
        '                End If
        '            Next
        '
        '        End If
    End Sub

    Private Sub YCM_3DView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode <> Keys.Control Then
            crl_mode = ControlMode.Normal
            blnRotateClick = False
        End If
        If e.KeyCode = Keys.Escape Then
            model_over = True
            model_pick = 0
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub YCM_3DView_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        Dim hits As Integer, viewport(4) As Integer
        Dim selectBuff(64) As UInteger
        Dim sx As Integer = Me.ClientSize.Height
        Me.Select()
        If m_blnCommandArea = False Then
            If crl_mode = ControlMode.Normal And e.Button = Windows.Forms.MouseButtons.Left Then
                '���̂�I������@�\�ŁA
                blnSelectMode = True
                glSelectBuffer(64, selectBuff)
                glGetIntegerv(GL_VIEWPORT, viewport)
                'glDisable(GL_LIGHTING)
                glMatrixMode(GL_PROJECTION)
                glPushMatrix()
                glRenderMode(GL_SELECT)
                glLoadIdentity()
                '�I��Matrix���쐬����
                gluPickMatrix(e.X, sx - e.Y, 7, 7, viewport)
                glOrtho(dblwindowLeft, dblwindowRight, dblwindowBottom, dblwindowTop, 0.0001, 100000)

                glMatrixMode(GL_MODELVIEW)
                Draw()
                '�I�����̖̂��O���擾����
                hits = glRenderMode(GL_RENDER)
                If hits > 0 Then
                    Dim blnPoint_ScreenSelect As Boolean = False
                    For ii As Integer = 0 To hits - 1
                        model_pick = selectBuff(3 + 4 * ii)
                        If model_pick >= 1 And model_pick < 10000 Then
                            blnPoint_ScreenSelect = True
                            Exit For
                        End If
                    Next
                    If blnPoint_ScreenSelect Then
                        For ii As Integer = 0 To hits - 1
                            model_pick = selectBuff(3 + 4 * ii)
                            If model_pick >= 1 And model_pick < 10000 Then Exit For
                        Next
                    Else
                        model_pick = selectBuff(3)
                    End If
                    If model_SingleSelct = False Then
                        model_picks.Add(model_pick)
                    End If
                End If
                glMatrixMode(GL_PROJECTION)
                glPopMatrix()
                glMatrixMode(GL_MODELVIEW)
                blnSelectMode = False
                Draw()
                'glEnable(GL_LIGHTING)
                'Dim geopoint2 As New GeoPoint
                'Dim gppoint As New GeoPoint
                'geopoint2.x = e.X
                'geopoint2.y = e.Y
                'YCM_PickPoint2TruePoint(e.X, e.Y, gppoint.x, gppoint.y, gppoint.z)
                ''IOUtil.GetPoint1(geopoint2, geopoint1)
                'MsgBox(gppoint.x & "," & gppoint.y & "," & gppoint.z)

                gYCM_MainFrame.SelectedImageDisp(c_click_camers.ID - 1)
            End If
        End If
    End Sub

    '======================================================================    
    ' �@�\�F�w�肳�ꂽ�݌v�_�Ɋ֘A���郉�x���A���C��`�悷��
    ' �����F
    '   iPos    [I/ ] �݌v�_�̃C���f�b�N�X
    '======================================================================    
    Private Sub drawSekkeiPoint(ByRef c_click As CLookPoint)
       
        Dim ii As Long, Mid As Long, iPosDraw As Integer
        Dim ���W�l�ꗗ�I����� As Boolean
        Dim bIsvisible As Boolean
        'c_click = New CLookPoint '20161114 baluu edit
        For ii = 0 To (nSekPoints - 1)   '�v���_��
            '[Type]�^�[�Q�b�g��� =1:�V���O���^�[�Q�b�g�A=2:�R�[�h�^�[�Q�b�g�A=9�F���[�U�ǉ��_
            bIsvisible = (gDrawSekPoints(ii).type = 1) And (entset_point.blnVisiable)
            bIsvisible = bIsvisible Or (gDrawSekPoints(ii).type = 2) And (CordTargetIsvisible)
            bIsvisible = bIsvisible Or (gDrawSekPoints(ii).type = 9) And (entset_pointUser.blnVisiable)
            'If (gDrawPoints(ii).flgLabel = 1) Then  '��\������\��
            If (bIsvisible = False) Then
                Mid = gDrawSekPoints(ii).mid
                ���W�l�ꗗ�I����� = True ' getDataVertexDrawFlag(Mid, ind)
                glLoadName(Point_Name + ii)

                If model_pick <> 0 Then
                    model_pick = model_pick
                End If
                If model_pick = Point_Name + ii Then
                    '�E�v���_�̕\���i�I����ԁj
                    iPosDraw = YCM_DrawLookPoint(gDrawSekPoints(ii), True)
                    c_click = gDrawSekPoints(ii)

                    '�E���x���\��
                    '  If (iPosDraw > 0) Then Call displayLabelByMID(Mid)
                Else
                    If (���W�l�ꗗ�I����� = True) Then
                        '�E�v���_�̕\���i��I����ԁj
                        iPosDraw = YCM_DrawLookPoint(gDrawSekPoints(ii), False)
                        '�E�v���_�̃��x����\������
                        ' If (iPosDraw > 0) Then Call displayLabelByMID(Mid)
                    Else
                        '�E���W�l�ꗗ�őI������Ă��Ȃ��v���_�̃��x���A���C�͕\�����Ȃ�
                    End If
                End If
                '20170220 baluu add start
                Dim wLabelText As New CLabelText
                wLabelText.blnDraw = True
                wLabelText.LabelName = gDrawSekPoints(ii).LabelName
                wLabelText.x = gDrawSekPoints(ii).x
                wLabelText.y = gDrawSekPoints(ii).y
                wLabelText.z = gDrawSekPoints(ii).z
                YCM_DrawLabelText(wLabelText, False)
                '20170220 baluu add end
            End If
        Next ii

    End Sub
    'Add CAST/2016-a3-20160920 Kiryu 3D�r���[����̐������폜
    'Drow()���̕`��ID:model_pick = �N���b�N�C�x���g���̕`��ID
    'blnDelONGlwin(�폜�t���O)
    Private Sub DeleteLineOnGlView()
        If nUserLines > 0 Then
            For ii As Integer = 0 To nUserLines - 1
                If gDrawUserLines(ii).model_pick = model_pick Then
                    gDrawUserLines(ii).blnDelOnGlwin = True
                End If
            Next
        End If
    End Sub
    Private Sub DeleteCircleOnGlView()
        If nCircleNew > 0 Then
            For ii As Integer = 0 To nCircleNew - 1
                If gDrawCircleNew(ii).model_pick = model_pick Then
                    gDrawCircleNew(ii).blnDelOnGlwin = True
                End If
            Next
        End If
    End Sub

    Private Sub �폜ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles �폜ToolStripMenuItem.Click
        Call DeleteLineOnGlView()
        Call DeleteCircleOnGlView()
    End Sub

    Public Sub ToggleSekkeiLine()

        DrawSekkeiLine = Not DrawSekkeiLine

    End Sub

End Class
